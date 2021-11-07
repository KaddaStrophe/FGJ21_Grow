using UnityEngine;

public class BackToMenu : MonoBehaviour {
    [SerializeField]
    GameObject pauseMenu = default;

    PlayerInput playerInput;

    protected void OnEnable() {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    protected void OnDisable() {
        playerInput.Disable();
    }

    protected void Start() {
        playerInput.Player.Pause.performed += ctx => PauseGame();
    }

    void PauseGame() {
        //if(!pauseMenu.activeSelf) {
        //    Time.timeScale = 0;
        //} else {
        //    Time.timeScale = 1;
        //}
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
}
