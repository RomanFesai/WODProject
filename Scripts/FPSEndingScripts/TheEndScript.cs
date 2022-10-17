using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEndScript : MonoBehaviour
{
    [SerializeField] private bool playerInRange = false;
    [SerializeField] private GameObject ActionKeyHint = default;
    public Animator transition;
    public Animator CarDoor;
    void Start()
    {
        
    }

    void Update()
    {
      if(playerInRange == true && Input.GetKeyDown(KeyCode.E))
        {
            CarDoor.SetBool("Open", true);
            StartCoroutine(LoadLevel("MainMenu"));
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == ("Player"))
        {
            playerInRange = true;
            ActionKeyHint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            playerInRange = false;
            ActionKeyHint.SetActive(false);
        }
    }

    IEnumerator LoadLevel(string levelname)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelname);
    }
}
