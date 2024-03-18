using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Properties;
using LT.Recall.Domain.Entities;
using static LT.Recall.Application.Features.Search.Response;

namespace LT.Recall.Application.Features
{
    public class Search
    {
        public class Request 
        {
            public string SearchString { get; init; } = string.Empty;
            public int PageSize { get; init; } = 20;
            public int Page { get; init; } = 1;
        }

        public class Response
        {
            public class SearchResult
            {
                public int CommandId { get; init; }
                public string CommandText { get; init; } = string.Empty;
                public string Description { get; init; } = string.Empty;
                public string Collection { get; init; } = string.Empty;
                public List<string> Tags { get; init; } = new();
            }

            public int Page { get; set; } = 1;
            public int TotalPages { get; set; } = 1;

            public int TotalResults { get; init; }

            public List<SearchResult> SearchResults { get; init; } = new();

            public string UserFriendlyMessage { get; init; } = string.Empty;
        }
        public class Handler 
        {
            private readonly ICommandRepository _commandRepository;
            public Handler(ICommandRepository commandRepository)
            {
                _commandRepository = commandRepository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                (List<Command>? commands, int totalResults) searchResults;

                if (string.IsNullOrWhiteSpace(request.SearchString))
                {
                    searchResults = await _commandRepository.FetchAllAsync(request.Page, request.PageSize);
                }
                else
                {
                    searchResults = await _commandRepository.SearchAsync(request.SearchString, request.Page, request.PageSize);
                }

                return new Response()
                {
                    TotalResults = searchResults.totalResults,
                    SearchResults = searchResults.commands?.Select(x => new SearchResult
                    {
                        CommandId = x.CommandId,
                        CommandText = x.CommandText,
                        Description = x.Description,
                        Tags = x.Tags.Select(x => x.Name).ToList(),
                        Collection = x.Collection
                    }).ToList() ?? new List<SearchResult>(),
                    UserFriendlyMessage = searchResults.totalResults > 0 ?
                        string.Format(Resources.CommandSearchResultsFoundMessage, searchResults.totalResults, request.SearchString)
                        : string.Format(Resources.CommandSearchResultsNotFoundMessage, request.SearchString),
                    Page = request.Page,
                    TotalPages = (int)Math.Ceiling((double)searchResults.totalResults / request.PageSize)
                };
            }
        }
    }
}
