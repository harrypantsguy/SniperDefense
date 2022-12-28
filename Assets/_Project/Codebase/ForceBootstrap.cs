#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;

namespace _Project.Codebase
{
    [InitializeOnLoad]
    public static class ForceBootstrap
    {
        static ForceBootstrap()
        {
            EditorApplication.playModeStateChanged += ForceLoadBootstrap;
        }

        private static void ForceLoadBootstrap(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
                SceneManager.LoadScene(0);
        }
    }
}
#endif