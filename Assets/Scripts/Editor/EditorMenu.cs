using UnityEditor;
using UnityEngine;

public class EditorMenu
{
    [MenuItem("Tools/Delete playerPrefs")]
    private static void DeletePlayerPrefs()
    {
        // видаляю всі збереженні дані
        PlayerPrefs.DeleteAll();
    }
}
