using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

public class FastSceneSwitcher : EditorWindow
{
    private static void OpenSceneByName(string sceneName)
    {
        string[] guids = AssetDatabase.FindAssets($"t:Scene {sceneName}");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string filename = Path.GetFileNameWithoutExtension(path);

            if (filename == sceneName)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(path);
                }
                return;
            }
        }

        EditorUtility.DisplayDialog("Escena no encontrada", $"No se encontró la escena \"{sceneName}\" en el proyecto.", "OK");
    }




    [MenuItem("Scene switch/Sample Scene")]
    private static void OpenMainMenu() => OpenSceneByName("SampleScene");

    [MenuItem("Scene switch/Sandbox")]
    private static void OpenHub() => OpenSceneByName("Sandbox");
}
