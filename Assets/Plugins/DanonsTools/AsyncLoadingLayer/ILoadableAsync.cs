using Cysharp.Threading.Tasks;

namespace DanonsTools.AsyncLoadingLayer
{
    public interface ILoadableAsync<T> : ILoadable
    {
        public UniTask<T> LoadAsync();
    }
}