using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Extensions;
using LT.Recall.Domain.Entities;
using MediatR;

namespace LT.Recall.Application.Features
{
    public class Stats
    {
        public class Request : IRequest<Response>
        {

        }

        public class Response
        {
            public class TotalsResponse
            {
                public int TotalCommands { get; set; }
                public string TotalSize { get; set; } = string.Empty;
            }

            public class TagsResponse
            {
                public string Tag { get; set; } = string.Empty;
                public int Count { get; set; }
                public string Size { get; set; } = string.Empty;
            }

            public class CollectionResponse
            {
                public string Collection { get; set; } = string.Empty;
                public int Count { get; set; }
                public string Size { get; set; } = string.Empty;
            }

            public TotalsResponse Totals { get; set; } = new();
            public List<TagsResponse> Tags { get; set; } = new();
            public List<CollectionResponse> Collections { get; set; } = new();
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ICommandRepository _commandRepository;

            public Handler(ICommandRepository commandRepository)
            {
                _commandRepository = commandRepository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var allCommands  = (await _commandRepository.FetchAllAsync(1, pageSize: int.MaxValue)).commands ?? new List<Command>();
                
                var response = new Response();
                
                GetTagSize(allCommands, response);
                GetTotalSize(response, allCommands);
                GetCollectionSize(allCommands, response);

                return response;
            }

            private static void GetTotalSize(Response response, List<Command> allCommands)
            {
                response.Totals.TotalSize = allCommands.Sum(command => command.Size).ToBytesDisplayValue();
                response.Totals.TotalCommands = allCommands.Count;
            }

            private static void GetCollectionSize(List<Command> allCommands, Response response)
            {
                foreach (var collection in allCommands.Select(x => x.Collection.ToLowerInvariant()).Distinct())
                {
                    var collectionCommands = allCommands.Where(command => string.Equals(command.Collection, collection, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    response.Collections.Add(new Response.CollectionResponse()
                    {
                        Collection = collection,
                        Count = collectionCommands.Count,
                        Size = (collectionCommands.Sum(command => command.Size)).ToBytesDisplayValue()
                    });
                }
                response.Collections = response.Collections.OrderByDescending(x => x.Count).ToList();
            }

            private static void GetTagSize(List<Command> allCommands, Response response)
            {
                foreach (var tag in allCommands.SelectMany(command => command.Tags.Select(tag => tag.Name.ToLowerInvariant())).Distinct())
                {
                    var tagCommands = allCommands.Where(command => command.Tags.Any(x => string.Equals(x.Name,tag, StringComparison.InvariantCultureIgnoreCase))).ToList();
                    response.Tags.Add(new Response.TagsResponse
                    {
                        Tag = tag,
                        Count = tagCommands.Count,
                        Size = (tagCommands.Sum(command => command.Size)).ToBytesDisplayValue()
                    });
                }
                response.Tags = response.Tags.OrderByDescending(x => x.Count).ToList();
            }
        }
    }
}
