using System.Collections;
using System.Threading.Tasks;
using LuaBridge.Core.Configuration;
using LuaBridge.Unity.Tests.Tests.UIServiceTest.Extensions;
using UnityEngine.TestTools;

namespace LuaBridge.Unity.Tests.Tests.UIServiceTest.Abstract
{
    public abstract class LuaBridgeTest
    {
        protected AppContainer AppContainer;


        [UnitySetUp]
        public virtual IEnumerator SetUp()
        {
            yield return CreateContainer().ToCoroutine(container => AppContainer = container);
        }

        protected abstract Task<AppContainer> CreateContainer();

        [UnityTearDown]
        public virtual IEnumerator TearDown()
        {
            AppContainer.Dispose();
            yield return null;
        }

    }
}