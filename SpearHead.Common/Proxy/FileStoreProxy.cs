using System;
using System.Net.Http;
using System.Threading.Tasks;
using SpearHead.FileStore.Models;
using SpearHead.Common.Helpers;
using System.Net.Http.Headers;

namespace SpearHead.Common.Proxy
{
    public sealed class FileStoreProxy : IFileStoreProxy
    {
        const string _fileBaseUrlKey = "FileStoreBaseUrl";
        private readonly string _baseUrl;

        public FileStoreProxy()
        {
            _baseUrl = ConfigHelper.GetAppSettingValue<string>(_fileBaseUrlKey);
        }

        public async Task<HttpResponseMessage> Post(FileModel model)
        {
            const string api = "filestore/api/upload";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return await client.PostAsJsonAsync(string.Format("{0}/{1}", _baseUrl, api), model);
            }
        }
    }
}
