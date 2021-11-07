using UnityEngine;

public class GameOver : MonoBehaviour {
    [SerializeField]
    GameObject gameOverCanvas = default;
    protected void OnEnable() {
        PlayerNavigation.onGameOver += PlayerNavigation_onGameOver;
    }
    protected void OnDisable() {
        PlayerNavigation.onGameOver -= PlayerNavigation_onGameOver;
    }

    void PlayerNavigation_onGameOver() {
        gameOverCanvas.SetActive(true);
    }
}
