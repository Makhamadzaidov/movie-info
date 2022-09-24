using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInfo.Data.Common
{
    public interface IDeleteable<T>
    {
        Task<bool> DeleteAsync(Int64 id);
    }
}
