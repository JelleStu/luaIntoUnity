using System.Threading.Tasks;
using LuaBridge.Core.Services.Abstract;
using Utils.Caching;

namespace Services
{
    public class AppCache : MemoryCache, IAsyncBootService
    {
        private readonly IFileService _fileService;
        private readonly string _path;

        public AppCache(IFileService fileService, string path)
        {
            _fileService = fileService;
            _path = path;
        }

        public async Task Boot()
        {
            await this.LoadFromDisk(_path, _fileService);
        }
    }
}