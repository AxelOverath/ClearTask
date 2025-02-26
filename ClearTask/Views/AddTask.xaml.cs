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
                        // Upload logic if needed
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
            List<string> generatedTags = await GenerateTagsFromAPI(description) ?? new List<string>();

            // Create a list of Tag objects from the generated tag names
            List<Tag> tagList = generatedTags.Select(tagName => new Tag
            {
                Id = ObjectId.GenerateNewId(),
                name = tagName,
                description = ""
            }).ToList();

            var newTask = new Task_
            {
                title = tasktitle.Text,
                description = description,
                photo = "https://example.com/lucas.jpg", // Save Image ID if needed
                tags = new List<ObjectId>(),  // Empty ObjectId list (update as necessary)
                taglist = tagList,  // Assign the generated tag list
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
            var enhancedPrompt = prompt + "\nReturn as JSON.";

            string ApiUri = AppConfig.ApiUri;

            using (var client = new HttpClient())
            {
                var requestBody = new
                {
                    model = "tagger",
                    prompt = enhancedPrompt,
                    stream = false,
                    options = new { temperature = 0 },
                    format = new
                    {
                        type = "object",
                        properties = new
                        {
                            generated_tags = new
                            {
                                type = "array",
                                items = new { type = "string" }
                            }
                        },
                        required = new[] { "generated_tags" }
                    }
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                try
                {
                    Console.WriteLine("Sending API request...");
                    Console.WriteLine($"Request Body: {JsonSerializer.Serialize(requestBody)}");

                    HttpResponseMessage response = await client.PostAsync($"{ApiUri}/api/generate", jsonContent);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response: {responseContent}");

                    var finalResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                    // Extract the "response" field (which is itself a JSON string)
                    if (finalResponse.TryGetProperty("response", out var responseElement))
                    {
                        string responseJson = responseElement.GetString();
                        Console.WriteLine($"Extracted Inner JSON: {responseJson}");

                        // Deserialize the inner JSON string
                        var innerResponse = JsonSerializer.Deserialize<JsonElement>(responseJson);

                        if (innerResponse.TryGetProperty("generated_tags", out var tagsElement))
                        {
                            List<string> tags = new List<string>();
                            foreach (var item in tagsElement.EnumerateArray())
                            {
                                tags.Add(item.GetString());
                            }

                            Console.WriteLine("Extracted Tags:");
                            tags.ForEach(tag => Console.WriteLine($"- {tag}"));

                            return tags;
                        }
                    }

                    Console.WriteLine("Property 'generated_tags' not found in extracted JSON.");
                    return new List<string> { "DefaultTag" };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"API Error: {ex.Message}");
                    await DisplayAlert("Error", $"Failed to generate tags: {ex.Message}", "OK");
                    return new List<string> { "DefaultTag" };
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
