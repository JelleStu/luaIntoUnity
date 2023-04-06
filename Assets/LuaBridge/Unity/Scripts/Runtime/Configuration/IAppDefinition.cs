using System;

namespace LuaBridge.Core.Configuration
{
    public interface IAppDefinition : IDisposable
    {
        public void Start(AbstractSceneContainer sceneContainer);
    }
}