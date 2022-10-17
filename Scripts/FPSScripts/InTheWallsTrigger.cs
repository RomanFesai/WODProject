using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTheWallsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { 
            FindObjectOfType<AudioManager>().Play("InTheWalls");
            FindObjectOfType<AudioManager>().Stop("Angels_song");
        }
    }
}
