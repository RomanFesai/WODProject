using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandUpTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovement standupTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            standupTrigger.StopCrouch();
            StartCoroutine(SelfDestroy());
        }
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.Find("StandUpTrigger"));
    }
}
