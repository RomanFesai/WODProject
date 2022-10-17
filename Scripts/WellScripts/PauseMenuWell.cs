using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuWell : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseWindow;
    //public MoveCamera PlayerCam;
    //public GameBehaviour IsLoss = default;

    void Start()
    {
        //PlayerCam = GameObject.Find("Camera").GetComponent<MoveCamera>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            { Pause(); }
        }
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        //PlayerCam.GetComponent<MoveCamera>().enabled = true;
        GameObject.Find("_Player").GetComponent<playerClimbRope>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        GameIsPaused = false;
        Cursor.visible = false;
        PauseWindow.SetActive(false);
    }

    public void Pause()
    {
        //PlayerCam.GetComponent<MoveCamera>().enabled = false;
        GameObject.Find("_Player").GetComponent<playerClimbRope>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        GameIsPaused = true;
        Cursor.visible = true;
        PauseWindow.SetActive(true);
    }
}
