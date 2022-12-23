using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DanonsTools.ContentLayer
{
    public interface IContentService
    {
        public UniTask LoadAssetGroupAsync<TGroup, TObject>() where TObject : Object where TGroup : IAssetGroup<TObject>;
        public UniTask<T> LoadAssetAsync<T>(string address) where T : Object;
        public T GetCachedAsset<T>(in string address) where T : Object;
    }
}