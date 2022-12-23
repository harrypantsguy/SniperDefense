using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace DanonsTools.ContentLayer
{
    public sealed class DefaultContentService : IContentService
    {
        private readonly Dictionary<string, Object> _cachedAddressables = new Dictionary<string, Object>();

        public async UniTask LoadAssetGroupAsync<TGroup, TObject>() where TObject : Object where TGroup : IAssetGroup<TObject>
        {
            foreach (var address in ContentUtilities.GetAssetAddressesInType(typeof(TGroup)))
                await LoadAssetAsync<TObject>(address);
        }

        public async UniTask<T> LoadAssetAsync<T>(string address) where T : Object
        {
            var asset = await Addressables.LoadAssetAsync<T>(address);
            _cachedAddressables.Add(address, asset);
            return asset;
        }

        public T GetCachedAsset<T>(in string address) where T : Object
        {
            if (_cachedAddressables.TryGetValue(address, out var asset))
                return asset as T;
            throw new Exception($"No asset with address {address} cached.");
        }
    }
}