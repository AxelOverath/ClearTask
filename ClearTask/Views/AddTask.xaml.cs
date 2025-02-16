using System.Runtime.InteropServices;
using ClearTask.Data;
using ClearTask.Models;
using ClearTask.ViewModels;
using MongoDB.Bson;
using TaskStatus = ClearTask.Models.TaskStatus;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace ClearTask.Views
{
    public partial class AddTask : ContentPage
    {
        private readonly TaskListViewModel _viewModel;
        private ObjectId? uploadedImageId = null;

        public AddTask()
        {
            InitializeComponent();
            _viewModel = new TaskListViewModel();
            BindingContext = _viewModel;
        }

        //  Handle Image Selection & Upload
        private async void OnPickImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select an image"
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                    }

                    await DisplayAlert("Success", "Image uploaded successfully!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
            }
        }

        //  Save Task with Image ID
        private async void OnSaveTaskClicked(object sender, EventArgs e)
        {
            string description = taskdescription.Text;
            // Fetch the list of generated tags from the API
            List<string> generatedTags = await GenerateTagsFromAPI(description);

            // Create Tag objects for each generated tag
            List<Tag> tagList = new List<Tag>();
            foreach (var tagName in generatedTags)
            {
                tagList.Add(new Tag
                {
                    Id = ObjectId.GenerateNewId(),
                    name = tagName,
                    description = ""
                });
            }

            var newTask = new Task_
            {
                title = tasktitle.Text,
                description = description,
                photo = "https://example.com/lucas.jpg", // Save Image ID
                taglist = tagList,
                deadline = DeadlinePicker.Date,
                status = TaskStatus.Pending,
                assignedTo = ObjectId.Empty,
                sector = ObjectId.Empty
            };

            await _viewModel.AddTask(newTask);
            await DisplayAlert("Success", "Task added successfully!", "OK");
            await Navigation.PopAsync();
        }
        private async Task<List<string>> GenerateTagsFromAPI(string prompt)
        {
            using (var client = new HttpClient())
            {
                var requestBody = new { model = "tagger", prompt = prompt };
                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync("http://localhost:11435/api/generate", jsonContent);
                    response.EnsureSuccessStatusCode(); // Throw an error if response is not 2xx

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Use JSON parsing instead of regex
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        var root = doc.RootElement;
                        List<string> tags = new List<string>();

                        // Check if the response contains the "response" field
                        if (root.TryGetProperty("response", out JsonElement responseElement))
                        {
                            // Split the response into tags (assuming it's a comma-separated list or multiple lines)
                            var responseString = responseElement.GetString();
                            var extractedTags = responseString?.Split(new[] { '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);

                            if (extractedTags != null)
                            {
                                foreach (var tag in extractedTags)
                                {
                                    string cleanedTag = tag.Trim();
                                    if (!string.IsNullOrEmpty(cleanedTag))
                                    {
                                        tags.Add(cleanedTag);
                                    }
                                }
                            }
                        }

                        // If no tags were found, fallback to a default tag
                        if (tags.Count == 0)
                        {
                            tags.Add("DefaultTag");
                        }

                        return tags;
                    }
                }
                catch (Exception ex)
                {
                    // Assuming DisplayAlert is defined in your environment (e.g., Xamarin.Forms, MAUI, etc.)
                    await DisplayAlert("Error", $"Failed to generate tags: {ex.Message}", "OK");
                    return new List<string> { "DefaultTag" }; // Fallback tag in case of an error
                }
            }
        }

        //  Back Button
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
