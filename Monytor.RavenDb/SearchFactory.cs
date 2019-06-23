using System.Collections.Generic;
using Monytor.Core.Models;
using Raven.Client;

namespace Monytor.RavenDb {
    public static class SearchFactory {
        public static Search<TResult> CreateSearchResult<TResult>(IList<TResult> items,
            RavenQueryStatistics queryStatistics, int page, int pageSize) {
            return new Search<TResult>() {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int) System.Math.Ceiling((double) queryStatistics.TotalResults / pageSize),
                TotalItems = queryStatistics.TotalResults
            };
        }
    }
}