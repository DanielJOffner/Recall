﻿using System.Net;
using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.IO;
using LT.Recall.Infrastructure.Properties;

namespace LT.Recall.Infrastructure.Installers.Url
{
    public class UrlInstaller :  IInstaller
    {
        public string Description => "Install collections from web resources";
        public string Name => "Url Installer";

        private readonly HttpClient _httpClient;
        private readonly TempFileInstaller _tempFileInstaller;

        public UrlInstaller(HttpClient httpClient, TempFileInstaller tempFileInstaller)
        {
            _httpClient = httpClient;
            _tempFileInstaller = tempFileInstaller;
        }


        public async Task<(int inserted, int updated)> Install(string collectionOrLocation)
        {
            using var writer = new TempFileWriter();
            var contents = await ReadContent(collectionOrLocation);
            var tempFile = writer.Write(contents, ".csv");
          
            return await _tempFileInstaller.Install(tempFile);
        }

        public async Task<string> ReadContent(string collectionOrLocation)
        {
            var response = await _httpClient.GetAsync(collectionOrLocation);
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InfrastructureError(string.Format(Resources.BadInstallUrlError, collectionOrLocation, response.StatusCode, content), InfraErrorCode.UnknownError);

            return content;
        }

        public Task<List<string>> ListCollections()
        {
            return Task.FromResult(new List<string>(){ "n/a" });
        }

    }
}
