using LT.Recall.Application.Abstractions;
using LT.Recall.Cli.Output;
using LT.Recall.Domain.Entities;
using LT.Recall.Infrastructure.Persistence.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LT.Recall.Cli.Serialization
{

    [JsonSerializable(typeof(State))]
    [JsonSerializable(typeof(Command))]
    [JsonSerializable(typeof(CliResult))]
    [JsonSerializable(typeof(Application.Features.Delete.Response), TypeInfoPropertyName = "DeleteResponse")]
    [JsonSerializable(typeof(Application.Features.Export.Response), TypeInfoPropertyName = "ExportResponse")]
    [JsonSerializable(typeof(Application.Features.Import.Response), TypeInfoPropertyName = "ImportResponse")]
    [JsonSerializable(typeof(Application.Features.Install.Response), TypeInfoPropertyName = "InstallResponse")]
    [JsonSerializable(typeof(Application.Features.ListInstallers.Response), TypeInfoPropertyName = "ListInstallersResponse")]
    [JsonSerializable(typeof(Application.Features.Save.Response), TypeInfoPropertyName = "SaveResponse")]
    [JsonSerializable(typeof(Application.Features.Search.Response), TypeInfoPropertyName = "SearchResponse")]
    [JsonSerializable(typeof(Application.Features.Stats.Response), TypeInfoPropertyName = "StatsResponse")]
    internal partial class SourceGenerationContext : JsonSerializerContext { }

    public class RecallJsonSerializer : IJsonSerializer
    {
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                TypeInfoResolver = SourceGenerationContext.Default
            });
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
        public T Deserialize<T>(string json) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(json))
            {   
               json = "{}";
            }

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                TypeInfoResolver = SourceGenerationContext.Default
            }) ?? new T();
        }
    }
}
