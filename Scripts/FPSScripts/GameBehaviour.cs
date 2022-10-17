using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class GameBehaviour : MonoBehaviour
{
    //public string labelText;
    public bool showWinScreen = false;
    public bool showLossScreen = false;
    public bool isGameOver = false;
    private int currentHealth; 
    public int _playerLives = 1;
    private string _state;
    private GUIStyle guiStyle = new GUIStyle();
    public Camera fpsCam;
    public HealthBar HealthBar;

    [SerializeField] private GameObject ScreamerHead;
    [SerializeField] private GameObject Flight;
    [SerializeField] private GameObject FHint;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject AmmoInfo;
    [SerializeField] private PlayerMovement crouch;
    public GameObject LossScreen;

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public int Lives
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            HealthBar.SetHealth(currentHealth);
            Debug.LogFormat("Lives: {0}", currentHealth);
            if (currentHealth <= 0)
            {
                UnityEngine.Cursor.visible = true;
                ShowLossScreen();
                Destroy(GameObject.Find("Capsule").GetComponent<PlayerMovement>());
            }
        }
    }
   /* void RestartLevel()
    {
        SceneManager.LoadScene("fps_lvl1");
        Time.timeScale = 1.0f;
        //PlayerMovement.StopCrouch();
    }*/

    void Start()
    {
        Initialize();
        currentHealth = _playerLives;
        HealthBar.SetMaxHealth(_playerLives);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        if (Utilities.playerDeaths > 0)
        {
            Destroy(ScreamerHead);
            FHint.SetActive(false);
            Flight.SetActive(true);
            Flashlight.FlashlightActive = true;
        }
    }

    public void Initialize()
    {
        _state = "Manager initialized..";
        Debug.Log(_state);
    }

  /*  void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.contentColor = Color.white;
        //GUI.Label(new Rect(45, 50, 150, 250), "" + _playerLives, guiStyle);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 600, 700), labelText, guiStyle);
        if (showWinScreen)
        {

        }
    }*/

    public void ShowLossScreen()
    {
        fpsCam.transform.rotation = new Quaternion(0, 0, 90, -90);
        gun.SetActive(false);
        AmmoInfo.SetActive(false);
        Time.timeScale = 0;
        LossScreen.SetActive(true);
        isGameOver = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        //PlayerCam.enabled = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        isGameOver = false;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        Utilities.RestartLevel();
        isGameOver = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
}
