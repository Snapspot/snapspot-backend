using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Shared.Common
{
    /// <summary>
    /// PagingResponse
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PagingResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingResponse{T}"/> class with paginated data and metadata.
        /// </summary>
        /// <param name="items">The items in the current page.</param>
        /// <param name="totalCount">The total number of items across all pages.</param>
        /// <param name="pageIndex">The current page index (starting from 1).</param>
        /// <param name="pageSize">The number of items per page.</param>
        public PagingResponse(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalItems = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
