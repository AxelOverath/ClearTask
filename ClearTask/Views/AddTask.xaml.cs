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
using MongoDB.Driver;
using Tag = ClearTask.Models.Tag;

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
            LoadUsers();
            LoadSectors();
        }
        private async void LoadUsers()
        {
            try
            {
                var users = await DatabaseService.UsersCollection.Find(user => user.userRole == Role.Handyman ).ToListAsync();

                if (users != null && users.Any())
                {
                    UserPicker.ItemsSource = users;
                    UserPicker.ItemDisplayBinding = new Binding("username");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
            }
        }
        private async void LoadSectors()
        {
            try
            {
                var sectors = await DatabaseService.SectorCollection.Find(_ => true).ToListAsync();

                if (sectors != null && sectors.Any())
                {
                    SectorPicker.ItemsSource = sectors;
                    SectorPicker.ItemDisplayBinding = new Binding("name");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load sectors: {ex.Message}", "OK");
            }
        }

        private async void OnSaveTaskClicked(object sender, EventArgs e)
        {
            // Ensure title and description are never null
            string title = tasktitle.Text?.Trim() ?? string.Empty;
            string description = taskdescription.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                await DisplayAlert("Validation Error", "Task title is required.", "OK");
                return;
            }

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
                title = title,
                description = description,
                photo = "https://example.com/lucas.jpg", // Save Image ID if needed
                tags = new List<ObjectId>(),  // Empty ObjectId list (update as necessary)
                taglist = tagList,  // Assign the generated tag list
                deadline = DeadlinePicker.Date,
                status = TaskStatus.Pending,
                assignedTo = (UserPicker.SelectedItem as User)?.Id ?? ObjectId.Empty,
                sector = (SectorPicker.SelectedItem as Sector)?.Id ?? ObjectId.Empty,
                createdBy = UserStorage.Id,
                isAdmin = AdminTicketCheckbox.IsChecked

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

        private void SetManagerVisibility(object sender, EventArgs e)
        {
            if (UserStorage.UserRole == Role.Manager)
            {
                // Show elements specific to the Manager role
                DeadlinePicker.IsVisible = true;
                AdminTicketCheckbox.IsVisible = true;
                UserPicker.IsVisible = true;
                this.FindByName<Label>("DeadlineLabel").IsVisible = true;
                this.FindByName<Label>("UserPickerLabel").IsVisible = true;
                this.FindByName<StackLayout>("AdminTicketStack").IsVisible = true;
            }
        }



        //  Back Button
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
