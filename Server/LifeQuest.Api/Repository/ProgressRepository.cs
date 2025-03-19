namespace LifeQuest.Api.Repository
{
    using System;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Models;

    public class ProgressRepository
    {
        private readonly BlobContainerClient containerClient;

        public ProgressRepository(string user)
        {
            // connection string fro setttings
            containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=lifegamificationstr;AccountKey=SzsMGpcD/ThVotAB8oYm8sYJXQJ8uWn1rS/5Py9T1qZXo3WcaGZPJClfIAbaN5wnjwW3gsw/polVYuQntEOmxw==;EndpointSuffix=core.windows.net",
            user);
        }

        public async Task<Progress> GetProgressAsync()
        {
            var blobClient = containerClient.GetBlobClient($"Progress.json");

            if (!await blobClient.ExistsAsync())
            {
                return null;
            }

            var result = await blobClient.DownloadContentAsync();
            return result.Value.Content.ToObjectFromJson<Progress>();
        }

        public async Task SetProgressAsync(Progress progress)
        {
            var blobClient = containerClient.GetBlobClient($"Progress.json");
            var data = new BinaryData(progress);
            await blobClient.UploadAsync(data, true);
        }
    }
}