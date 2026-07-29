using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Common
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }

        public int PageSize { get;  }
        public int PageIndex { get; }
        public int Count { get; }
        public IReadOnlyList<T> Data { get;}
    }
}
