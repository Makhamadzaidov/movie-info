using MovieInto.Domain.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInfo.Data.Common
{
    public interface IReadable<T>
    {
        Task<T> ReadAsync(Int64 id);

        Task<IEnumerable<T>> ReadAllAsync(PaginationParams @params);
    }
}
