using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private void Awake()
    {
        Debug.unityLogger.logEnabled = false;
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
