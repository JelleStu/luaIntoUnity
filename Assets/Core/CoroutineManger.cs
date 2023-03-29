using System;
using MoonSharp.Interpreter;

namespace Luncay.Core
{
    public class CoroutineManger
    {
        private FakeCoreModule _fakeCoreModule;

        public CoroutineManger(FakeCoreModule fakeCoreModule)
        {
            _fakeCoreModule = fakeCoreModule;
        }
    }
}