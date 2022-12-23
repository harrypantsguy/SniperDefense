using Cysharp.Threading.Tasks;

namespace DanonsTools.AsyncLoadingLayer
{
    public interface ILoadableAsyncEnumerable<T> : ILoadable
    {
        public IUniTaskAsyncEnumerable<T> LoadAsyncEnumerable();
    }
}