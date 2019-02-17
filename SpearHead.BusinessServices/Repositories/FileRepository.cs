using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SpearHead.Common.Helpers;

namespace SpearHead.BusinessServices.Repositories
{
    public sealed class FileRepository : IStorageRepsitory
    {
        private string _baseLocation;

        public void Configure(string location)
        {
            _baseLocation = location;

        }

        public async Task<bool> Move(string currentLocation)
        {
            string fileName = FileHelper.GetFileName(currentLocation);
            try
            {
                await Task.Run(() =>
               {
                   File.Move(currentLocation, $"{_baseLocation}\\{fileName}");

               });
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
