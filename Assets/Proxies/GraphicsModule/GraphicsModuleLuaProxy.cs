using Luncay.Core;
using MoonSharp.Interpreter;

namespace Proxies.GraphicsModule
{
    public class GraphicsModuleLuaProxy
    {
        private Script _scriptTarget;

        [MoonSharpHidden]
        public GraphicsModuleLuaProxy(Script target)
        {
            _scriptTarget = target;
        }

        public void Update()
        {
            _scriptTarget.Globals.Get("Update");
            
        }
        
        
    }
}