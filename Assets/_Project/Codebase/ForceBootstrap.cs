#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor;

[InitializeOnLoadAttribute]
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
#endif