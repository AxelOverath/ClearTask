using MongoDB.Driver;
using ClearTask.Models;
using MongoDB.Bson;
using CustomTag = ClearTask.Models.Tag;
using System.Text.Json;
namespace ClearTask.Data
{
    internal class DatabaseService
    {
        private static IMongoCollection<Task_> taskCollection;
        private static IMongoCollection<Sector> sectorCollection;
        private static IMongoCollection<CustomTag> tagCollection;
        private static IMongoCollection<User> userCollection;
        private static CancellationTokenSource _cts = new();

        public static event Action UserUpdated;
        public static event Action SectorUpdated;

        static DatabaseService()
        {
            const string connectionUri = "mongodb://axeloverath:Lao1KgIFn9WJeofd@cleartask-shard-00-00.dcnmf.mongodb.net:27017,cleartask-shard-00-01.dcnmf.mongodb.net:27017,cleartask-shard-00-02.dcnmf.mongodb.net:27017/?ssl=true&replicaSet=atlas-ah0h9j-shard-0&authSource=admin&retryWrites=true&w=majority&appName=ClearTask";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            string databaseName = "mongodbVSCodePlaygroundDB";
            var client = new MongoClient(settings);
            var db = client.GetDatabase(databaseName);

            taskCollection = db.GetCollection<Task_>("tasks");
            sectorCollection = db.GetCollection<Sector>("sectors");
            tagCollection = db.GetCollection<CustomTag>("tags");
            userCollection = db.GetCollection<User>("users");
            StartUserChangedListener();
        }

        public static IMongoCollection<Task_> TaskCollection => taskCollection;

        public static async Task InsertTaskAsync(Task_ task)
        {
            await taskCollection.InsertOneAsync(task);
        }
        public static IMongoCollection<User> UsersCollection
        {
            get { return userCollection; }
        }
        public static IMongoCollection<Sector> SectorsCollection
        {
            get { return sectorCollection; }
        }

        // Fetch Handyman object by ObjectId
        public static async Task<Handyman> GetHandymanById(ObjectId userId)
        {
            var user = await userCollection.Find(u => u.Id == userId && u.userRole == Role.Handyman).FirstOrDefaultAsync();

            if (user == null)
            {
                Console.WriteLine($"No Handyman found for ID: {userId}");
                return null;
            }

            // Convert User to Handyman (if Handyman is a subclass)
            return new Handyman
            {
                Id = user.Id,
                username = user.username,
                // Map additional handyman-specific fields if needed
            };
        }


        // Fetch Sector object by ObjectId
        public static async Task<Sector> GetSectorById(ObjectId sectorId)
        {
            var sector = await sectorCollection.Find(s => s.Id == sectorId).FirstOrDefaultAsync();
            return sector;
        }

        // Fetch Tag object by ObjectId
        public static async Task<CustomTag> GetTagById(ObjectId tagId)
        {
            var tag = await tagCollection.Find(t => t.Id == tagId).FirstOrDefaultAsync();
            return tag;
        }

        // Fetch tags based on list of ObjectIds
        public static async Task<List<CustomTag>> GetTagsByIds(List<ObjectId> tagIds)
        {
            var tags = await tagCollection.Find(t => tagIds.Contains(t.Id)).ToListAsync();
            return tags;
        }

        public static async Task InsertUserAsync(User user)
        {
            await userCollection.InsertOneAsync(user);
        }

        public static async Task<List<User>> GetAllUsersAsync()
        {
            return await userCollection.Find(_ => true).ToListAsync();
        }

        public static async Task<User> GetCurrentUserAsync()
        {
            return await SecureStorage.GetAsync("currentUser") is string userData
                ? JsonSerializer.Deserialize<User>(userData)
                : null;
        }

        // Method to populate related data for a task
        public static async Task<Task_> GetTaskWithDetails(ObjectId taskId)
        {
            // Fetch the Task
            var task = await taskCollection.Find(t => t.Id == taskId).FirstOrDefaultAsync();

            if (task != null)
            {
                // Populate 'assignedTo' (Handyman)
                if (task.assignedTo != null)
                {
                    var handyman = await GetHandymanById(task.assignedTo);
                    task.hassignedTo = handyman; // Assign the related Handyman object
                }

                // Populate 'sector' (Sector)
                if (task.sector != null)
                {
                    var sector = await GetSectorById(task.sector);
                    task.actualSector = sector; // Assign the related Sector object
                }

                // Populate 'taglist' (Tags)
                if (task.tags != null && task.tags.Any())
                {
                    var tagIds = task.tags.ToList();
                    Console.WriteLine($"Tag IDs for Task {task.Id}: {string.Join(", ", tagIds)}"); // Debugging

                    task.taglist = await GetTagsByIds(tagIds); // Assign related Tags

                    Console.WriteLine($"Retrieved {task.taglist.Count} tags for Task {task.Id}"); // Debugging
                }

            }

            return task;
        }

    private static async void StartUserChangedListener()
    {
        Console.WriteLine("Listening for task changes...");

        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<User>>()
            .Match(change => change.OperationType == ChangeStreamOperationType.Insert ||
                            change.OperationType == ChangeStreamOperationType.Update ||
                            change.OperationType == ChangeStreamOperationType.Delete);

        var cursor = await userCollection.WatchAsync(pipeline, cancellationToken: _cts.Token);

        _ = Task.Run(async () =>
        {
            try
            {
                while (await cursor.MoveNextAsync(_cts.Token))
                {
                    foreach (var change in cursor.Current)
                    {
                        Console.WriteLine($"Database change detected: {change.OperationType}");
                        UserUpdated?.Invoke();
                    }
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Change stream listening stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in change stream listener: {ex.Message}");
            }
        });
        }
        public static void StopListening()
        {
            _cts.Cancel();
        }

        public static void TriggerUserUpdatedEvent()
        {
            UserUpdated?.Invoke();
        }

        public static void TriggerSectorUpdatedEvent()
        {
            SectorUpdated?.Invoke();
        }
    }
}
