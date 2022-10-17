using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject player;
    public GameObject KeyCharTrigger;

    public void OnMouseEnter()
    {
        MouseControl.instance.Clickable();
    }
    public void OnMouseExit()
    {
        MouseControl.instance.Default();
    }

    private void OnMouseDown()
    {
        MouseControl.instance.Default();
        Destroy(GameObject.Find("key"));
        Invoke("DestroyQT", 2f);
        FindObjectOfType<AudioManager>().Play("KeyPickUp");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void DestroyQT()
    {
        player.SetActive(true);
        Destroy(GameObject.Find("Trigger"));
        Destroy(GameObject.Find("Head3.1"));
    }

}
