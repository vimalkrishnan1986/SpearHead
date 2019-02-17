using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.BusinessServices.Repositories
{
    public interface IStorageRepsitory
    {
        void Configure(string location);
        Task<bool> Move(string currentLocation);
    }
}
