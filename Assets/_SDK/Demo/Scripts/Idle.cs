using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(LoadScene), 1.5f);
    }

    void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)SceneID.Loading, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
