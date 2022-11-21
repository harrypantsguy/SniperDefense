using UnityEngine;

namespace _Project.Codebase
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Singleton { get; private set; }

        protected virtual void Awake()
        {
            Singleton = (T)this;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            Singleton = null;
        }
    }
}