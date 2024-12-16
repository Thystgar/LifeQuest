namespace LifeQuest.Api.Repository
{
    using Azure.Storage.Blobs;
    using LifeQuest.Api.Models;

    public class TaskTypesRepository
    {
        private readonly BlobContainerClient containerClient;

        public TaskTypesRepository(string user)
        {
            // connection string fro setttings
            containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=lifegamificationstr;AccountKey=SzsMGpcD/ThVotAB8oYm8sYJXQJ8uWn1rS/5Py9T1qZXo3WcaGZPJClfIAbaN5wnjwW3gsw/polVYuQntEOmxw==;EndpointSuffix=core.windows.net",
            user);
        }

        public async Task<List<TaskType>> GetTaskTypesAsync()
        {
            var blobClient = containerClient.GetBlobClient($"TaskTypes.json");

            if (!await blobClient.ExistsAsync())
            {
                return new List<TaskType>();
            }

            var result = await blobClient.DownloadContentAsync();
            return result.Value.Content.ToObjectFromJson<List<TaskType>>();
        }

        public async Task SetTaskTypesAsync(List<TaskType> taskTypes)
        {
            var blobClient = containerClient.GetBlobClient($"TaskTypes.json");
            var data = new BinaryData(taskTypes);
            await blobClient.UploadAsync(data, true);
        }
    }
}
