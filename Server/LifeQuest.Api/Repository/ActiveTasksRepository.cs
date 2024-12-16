namespace LifeQuest.Api.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using LifeQuest.Api.Models;
    using Azure.Identity;

    public class ActiveTasksRepository
    {
        private readonly BlobContainerClient containerClient;

        public ActiveTasksRepository(string user)
        {
            containerClient = new BlobContainerClient("", "");

            containerClient.CreateIfNotExists();
        }

        public async Task<List<ActiveTask>?> GetActiveTasksAsync(DateTime day)
        {
            var blobClient = containerClient.GetBlobClient($"ActiveTasks_{day.Date}.json");

            if (!await blobClient.ExistsAsync())
            {
                return null;
            }

            var result = await blobClient.DownloadContentAsync();
            return result.Value.Content.ToObjectFromJson<List<ActiveTask>>();
        }

        public async Task SetActiveTasksAsync(DateTime day, List<ActiveTask> tasks)
        {
            var blobClient = containerClient.GetBlobClient($"ActiveTasks_{day.Date}.json");
            // set ttl 1 month

            var data = new BinaryData(tasks);
            await blobClient.UploadAsync(data, true);
        }
    }
}
