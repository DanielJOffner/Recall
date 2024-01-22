using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.Properties;
using System.Net;

namespace LT.Recall.Infrastructure.Installers.Github
{
    public class GitHubClient
    {
        private readonly HttpClient _httpClient;

        private const string BasePath = "https://raw.githubusercontent.com/DanielJOffner/Recall/installers/collections/";
        private const string FileExtension = ".csv";

        public GitHubClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Exists(string collectionOrLocation)
        {
            var url = GetUrl(collectionOrLocation);
            var response = await _httpClient.GetAsync(url);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return false;
                case HttpStatusCode.OK:
                    return true;
                default:
                    throw new InfrastructureError(string.Format(Resources.UnknownError, await response.Content.ReadAsStringAsync()), InfraErrorCode.UnknownError);
            }
        }

        public async Task<(string contents, string extension)> GetContents(string collectionOrLocation)
        {
            var url = GetUrl(collectionOrLocation);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if(response.StatusCode != HttpStatusCode.OK)
                throw new InfrastructureError(string.Format(Resources.UnknownError, content), InfraErrorCode.UnknownError);

            return (content, FileExtension);
        }

        public async Task<List<string>> ListCollections()
        {
            var url = GetUrl("");
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return new List<string>() { };
        }


        private string GetUrl(string collectionOrLocation)
        {
            return $"{BasePath}{collectionOrLocation}{FileExtension}";
        }
    }
}
