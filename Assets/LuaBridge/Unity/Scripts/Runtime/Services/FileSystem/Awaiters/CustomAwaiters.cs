using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Services.Awaiters
{
    [DebuggerNonUserCode]
    public readonly struct AsyncOperationAwaiter : INotifyCompletion
    {
        private readonly AsyncOperation _asyncOperation;
        public bool IsCompleted => _asyncOperation.isDone;

        public AsyncOperationAwaiter(AsyncOperation asyncOperation) => _asyncOperation = asyncOperation;

        public void OnCompleted(Action continuation) => _asyncOperation.completed += _ => continuation();

        public void GetResult()
        {
        }
    }

    [DebuggerNonUserCode]
    public readonly struct UnityWebRequestAwaiter : INotifyCompletion
    {
        private readonly UnityWebRequestAsyncOperation _asyncOperation;

        public bool IsCompleted => _asyncOperation.isDone;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOperation) => _asyncOperation = asyncOperation;

        public void OnCompleted(Action continuation) => _asyncOperation.completed += _ => continuation();

        public UnityWebRequest GetResult() => _asyncOperation.webRequest;
    }

    [DebuggerNonUserCode]
    public readonly struct AssetBundleCreateRequestAwaiter : INotifyCompletion
    {
        private readonly AssetBundleCreateRequest _asyncOperation;

        public bool IsCompleted => _asyncOperation.isDone;

        public AssetBundleCreateRequestAwaiter(AssetBundleCreateRequest asyncOperation) => _asyncOperation = asyncOperation;

        public void OnCompleted(Action continuation) => _asyncOperation.completed += _ => continuation();

        public AssetBundleCreateRequest GetResult() => _asyncOperation;
    }


    [DebuggerNonUserCode]
    public readonly struct AssetBundleRequestAwaiter : INotifyCompletion
    {
        private readonly AssetBundleRequest _asyncOperation;

        public bool IsCompleted => _asyncOperation.isDone;

        public AssetBundleRequestAwaiter(AssetBundleRequest asyncOperation) => _asyncOperation = asyncOperation;

        public void OnCompleted(Action continuation) => _asyncOperation.completed += _ => continuation();

        public AssetBundleRequest GetResult() => _asyncOperation;
    }

    [DebuggerNonUserCode]
    public readonly struct ResourceRequestAwaiter : INotifyCompletion
    {
        private readonly ResourceRequest _asyncOperation;

        public bool IsCompleted => _asyncOperation.isDone;

        public ResourceRequestAwaiter(ResourceRequest asyncOperation) => _asyncOperation = asyncOperation;

        public void OnCompleted(Action continuation) => _asyncOperation.completed += _ => continuation();

        public Object GetResult() => _asyncOperation.asset;
    }

    [DebuggerNonUserCode]
    public readonly struct ResourceRequestAwaiter<T> : INotifyCompletion where T : Object
    {
        private readonly ResourceRequest _asyncOperation;

        public bool IsCompleted => _asyncOperation.isDone;

        public ResourceRequestAwaiter(ResourceRequest asyncOperation) => _asyncOperation = asyncOperation;

        public void OnCompleted(Action continuation) => _asyncOperation.completed += _ => continuation();

        public T GetResult() => _asyncOperation.asset as T;
    }


    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }

        public static AssetBundleCreateRequestAwaiter GetAwaiter(this AssetBundleCreateRequest asyncOp)
        {
            return new AssetBundleCreateRequestAwaiter(asyncOp);
        }

        public static AssetBundleRequestAwaiter GetAwaiter(this AssetBundleRequest asyncOp)
        {
            return new AssetBundleRequestAwaiter(asyncOp);
        }

        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest asyncOp)
        {
            return new ResourceRequestAwaiter(asyncOp);
        }
    }
}