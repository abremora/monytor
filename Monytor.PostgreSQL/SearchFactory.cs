using System.Collections.Generic;
using System.Linq;
using Marten.Pagination;
using Monytor.Core.Models;

namespace Monytor.PostgreSQL
{
    public static class SearchFactory
    {
        public static Search<TResult> CreateSearchResult<TResult>(IPagedList<TResult> pagedList)
        {
            return new Search<TResult>()
            {
                Items = pagedList.ToList(),
                Page = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                TotalPages = pagedList.PageCount,
                TotalItems = pagedList.TotalItemCount
            };
        }
    }
}