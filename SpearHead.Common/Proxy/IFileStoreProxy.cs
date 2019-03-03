using System.Net.Http;
using System.Threading.Tasks;
using SpearHead.FileStore.Models;

namespace SpearHead.Common.Proxy
{
    public interface IFileStoreProxy
    {
        Task<HttpResponseMessage> Post(FileModel model);
    }
}
