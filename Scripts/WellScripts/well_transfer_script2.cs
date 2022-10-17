using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class well_transfer_script2 : MonoBehaviour
{
    [SerializeField] private bool playerInRange = false;
    public Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("ForestTheme");
        FindObjectOfType<AudioManager>().Play("WellTheme");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            StartCoroutine(LoadLevel("fps_lvl1"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    IEnumerator LoadLevel(string levelname)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelname);
    }
}
