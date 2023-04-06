using System;
using System.Threading.Tasks;

namespace LuaBridge.Core.Services.Abstract
{
    public interface IAsyncBootService : IDisposable
    {
        public Task Boot();
    }
}