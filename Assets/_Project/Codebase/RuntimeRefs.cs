using UnityEngine;

namespace _Project.Codebase
{
    public class RuntimeRefs : MonoSingleton<RuntimeRefs>
    {
        [field: SerializeField] public Canvas ScreenSpaceCanvas { get; private set; }
        [field: SerializeField] public Canvas WorldSpaceCanvas { get; private set; }
    }
}