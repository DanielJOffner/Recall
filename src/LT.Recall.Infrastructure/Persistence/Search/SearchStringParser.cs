using LT.Recall.Infrastructure.Persistence.Models;
using System.Text.RegularExpressions;

namespace LT.Recall.Infrastructure.Persistence.Search
{
    internal static class SearchStringParser
    {
        public static SearchModel Parse(string searchString)
        {
            var searchModel = new SearchModel();

            if (string.IsNullOrWhiteSpace(searchString))
            {
                return searchModel;
            }

            searchString = Regex.Replace(searchString, @"\s+", " ");
            searchModel.Terms = searchString.Trim().Split(' ').ToList();
            return searchModel;
        }
    }
}
