using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareScriptTrigger : MonoBehaviour
{
    [SerializeField] private GameObject ScreamerHead;
    [SerializeField] private GameObject Flight;
    [SerializeField] private GameObject FHint;
    private bool jumpscareStarted = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && jumpscareStarted == false)
        {
            FHint.SetActive(false);
            jumpscareStarted = true;
            FindObjectOfType<AudioManager>().Play("Jumpscare");
            StartCoroutine(JumpScareDestroy());
        }
    }
   
    IEnumerator JumpScareDestroy()
    {
        yield return new WaitForSeconds(5);
        Flight.SetActive(false);
        Flashlight.FlashlightActive = false;
        FindObjectOfType<AudioManager>().Play("FlashLightClick");
        Destroy(ScreamerHead);
    }
}
