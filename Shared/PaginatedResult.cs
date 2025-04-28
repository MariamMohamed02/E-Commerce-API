using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    // generic because we might have a page for product, then brands, etc... so can't put the type directly
    public record PaginatedResult<TData>(int pageSize, int pageIndex, int totalCount, IEnumerable<TData> Data)
    {

    }
}
