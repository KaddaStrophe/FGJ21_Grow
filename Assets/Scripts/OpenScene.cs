using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour
{
    [SerializeField]
    string sceneName = default;

    public void LoadScene() {
        SceneManager.LoadScene(sceneName);
    }
}
