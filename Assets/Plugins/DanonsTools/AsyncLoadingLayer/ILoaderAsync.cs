using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace DanonsTools.AsyncLoadingLayer
{
    public interface ILoaderAsync<T> where T : ILoadable
    {
        public int LoadIndex { get; }
        public int LoadLength { get; }
        public IEnumerable<T> LoadQueue { get; }
        public void EnqueueLoadable(in T loadable);
        public void EnqueueLoadables(in IEnumerable<T> loadables);
        public UniTask ProcessQueue();
        public IUniTaskAsyncEnumerable<T> ProcessQueueEnumerable();
    }
}