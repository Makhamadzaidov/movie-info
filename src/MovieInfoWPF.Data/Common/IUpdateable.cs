using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInfo.Data.Common
{
    public interface IUpdateable<T>
    {
        Task<bool> UpdateAsync(Int64 id, T entity);
    }
}
