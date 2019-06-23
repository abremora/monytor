using System;
using System.Collections.Generic;
using System.Text;

namespace Monytor.Core.Models {
    public class Search<TResult> {
        public IList<TResult> Items { get; set; }
        public long Page { get; set; }
        public long PageSize { get; set; }
        public long TotalItems { get; set; }
        public long TotalPages { get; set; }
    }
}
