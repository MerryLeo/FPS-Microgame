using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    GameObject gameOverScreen, pauseScreen, playerUI, enemyUI;

    GameObject _playerObj;
    FPSCameraController _playerFPSCameraControllerScript;
    PlayerController_FSM _playerControllerScript;
    PlayerStatus _playerStatusScript;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1;
        _playerObj = GameObject.Find("Player");
        _playerFPSCameraControllerScript = _playerObj.GetComponentInChildren<FPSCameraController>();
        _playerControllerScript = _playerObj.GetComponent<PlayerController_FSM>();
        _playerStatusScript = _playerObj.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update() {
        if (_playerStatusScript.IsDead) {
            GameOver();
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver() {
        playerUI.SetActive(false);
        enemyUI.SetActive(false);
        gameOverScreen.SetActive(true);
        _playerControllerScript.disableControl = true;
        _playerFPSCameraControllerScript.DisableCameraControl();
        Time.timeScale = 0;
    }

    public void Pause() {
        pauseScreen.SetActive(true);
        playerUI.SetActive(false);
        enemyUI.SetActive(false);
        _playerControllerScript.disableControl = true;
        _playerFPSCameraControllerScript.DisableCameraControl();
        Time.timeScale = 0;
    }

    public void Resume() {
        pauseScreen.SetActive(false);
        playerUI.SetActive(true);
        enemyUI.SetActive(true);
        _playerControllerScript.disableControl = false;
        _playerFPSCameraControllerScript.EnableCameraControl();
        Time.timeScale = 1;
    }

    public void Quit() {
        Application.Quit();
    }
}
