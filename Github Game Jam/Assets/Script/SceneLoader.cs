using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    string targetScene = "";

    public void Load()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(targetScene);
    }
}
