using UnityEngine;

public class ExitGame : MonoBehaviour {
    public void QuitGame() {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
