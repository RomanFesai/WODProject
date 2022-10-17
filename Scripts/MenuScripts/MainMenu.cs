using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        Screen.SetResolution(800, 600, true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        FindObjectOfType<AudioManager>().Stop("ForestTheme");
        FindObjectOfType<AudioManager>().Stop("InTheWalls");
        FindObjectOfType<AudioManager>().Stop("WellTheme");
        FindObjectOfType<AudioManager>().Stop("Angels_song");
        FindObjectOfType<AudioManager>().Stop("EmptyAir");
        FindObjectOfType<AudioManager>().Play("MainMenuTheme");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        FindObjectOfType<AudioManager>().Stop("MainMenuTheme");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
