using _Project.Content.Prefabs;
using Cysharp.Threading.Tasks;
using DanonsTools.ContentLayer;
using DanonsTools.ServiceLayer;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Codebase
{
    public class GameBootstrap : MonoBehaviour
    {
        [UsedImplicitly]
        private async UniTaskVoid Start()
        {
            ServiceLocator.Initialize();
            ServiceLocator.Bind<IContentService, DefaultContentService>(new DefaultContentService());
            var contentService = ServiceLocator.Retrieve<IContentService>();

            await contentService.LoadAssetGroupAsync<PrefabAssetGroup, GameObject>();
            SceneManager.LoadScene(1);
        }
    }
}