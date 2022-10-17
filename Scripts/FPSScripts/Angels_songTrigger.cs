using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angels_songTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("Angels_song");
            FindObjectOfType<AudioManager>().Stop("InTheWalls");
        }
    }
}
