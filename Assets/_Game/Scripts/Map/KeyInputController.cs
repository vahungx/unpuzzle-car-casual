using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class KeyInputController
{
#if QUICKKEYMODE
    [MenuItem("MyMenu/Do Something with a Shortcut Key _a")]
    public static void DoSomethingWithAShortcutKey()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Clear PlayerPrefs");
    }

    [MenuItem("MyMenu/Do Something with q Shortcut Key _q")]
    public static void DoSomethingWithQShortcutKey()
    {
        DataMananger.instance.LoadData();
        DataMananger.instance.gameSave.currentLevel--;
        DataMananger.instance.SaveGame();
        SceneManager.LoadScene((int)SceneID.Gameplay);
        Debug.Log("Press Q to Back LastLevel");
    }

    [MenuItem("MyMenu/Do Something with e Shortcut Key _e")]
    public static void DoSomethingWithEShortcutKey()
    {
        DataMananger.instance.LoadData();
        DataMananger.instance.gameSave.currentLevel++;
        DataMananger.instance.SaveGame();
        SceneManager.LoadScene((int)SceneID.Gameplay);
        Debug.Log("Press E to Up Level");
    }
#endif
#if EDITORMODE
    // AWSDQEF
    [MenuItem("MyMenu/Do Something with a Shortcut Key _a")]
    public static void DoSomethingWithAShortcutKey()
    {
        MapEditor.Instance.Left();
        Debug.Log("Press A to Choose Left");
    }

    [MenuItem("MyMenu/Do Something with s Shortcut Key _s")]
    public static void DoSomethingWithSShortcutKey()
    {
        MapEditor.Instance.Down();
        Debug.Log("Press S to Choose Down");
    }

    [MenuItem("MyMenu/Do Something with d Shortcut Key _d")]
    public static void DoSomethingWithDShortcutKey()
    {
        MapEditor.Instance.Right();
        Debug.Log("Press D to Choose Right");
    }

    [MenuItem("MyMenu/Do Something with w Shortcut Key _w")]
    public static void DoSomethingWithWShortcutKey()
    {
        MapEditor.Instance.Up();
        Debug.Log("Press W to Choose Up");
    }

    [MenuItem("MyMenu/Do Something with q Shortcut Key _q")]
    public static void DoSomethingWithQShortcutKey()
    {
        MapEditor.Instance.Empty();
        Debug.Log("Press Q to Choose Empty");
    }

    //[MenuItem("MyMenu/Do Something with f Shortcut Key _f")]
    //public static void DoSomethingWithfShortcutKey()
    //{
    //    MapEditor.Instance.Box();
    //    Debug.Log("Press F to Choose Box");
    //}
#endif
}
