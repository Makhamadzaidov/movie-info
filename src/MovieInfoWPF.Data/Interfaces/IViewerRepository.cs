using MovieInfo.Data.Common;
using MovieInto.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInfo.Data.Interfaces
{
    public interface IViewerRepository:
         ICreateable<Viewer>, IReadable<Viewer>, IUpdateable<Viewer>, IDeleteable<Viewer>
    {

    }
}
