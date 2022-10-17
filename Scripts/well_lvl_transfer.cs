using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class well_lvl_transfer : MonoBehaviour
{
    [SerializeField] private static bool key;
    [SerializeField] private bool playerInRange = false;
    [SerializeField] private GameObject WellCover = default;
    [SerializeField] private GameObject WellCover2 = default;
    [SerializeField] private GameObject Lock = default;
    [SerializeField] private GameObject Lockedhint = default;
    [SerializeField] private GameObject ActionKeyHint = default;
    public Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("ForestTheme");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange == true && Input.GetKeyDown(KeyCode.X) && KeyCharMercyEvent.have_well_key == true)
        {
            WellCover.SetActive(false);
            WellCover2.SetActive(true);
            Lock.SetActive(false);
            StartCoroutine(LoadLevel("well_transfer"));
        }
        else if(playerInRange == true && Input.GetKeyDown(KeyCode.X)) 
        {
            Lockedhint.SetActive(true);
            StartCoroutine(isLocked());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            ActionKeyHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
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

    IEnumerator isLocked()
    {
        yield return new WaitForSeconds(0.2f);
        Lockedhint.SetActive(false);
    }
}
