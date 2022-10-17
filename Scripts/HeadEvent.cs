using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEvent : MonoBehaviour
{
    public GameObject HeadEvent2;
    public Animator HeadPunch;
    private bool MidAttack = false;
    [SerializeField] private GameObject Xhinthead;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!MidAttack) 
            { 
                Action1();
                Xhinthead.SetActive(false);
            }
            else
            {
                Action2();
            }
        }
    }

    void Action1()
    {
        HeadPunch.SetBool("MidAttack", true);
        MidAttack = true;
    }

    void Action2()
    {
        HeadPunch.SetBool("HardAttack", true);
        Invoke("DestroyQT", 2f);
    }

    void DestroyQT()
    {
        HeadEvent2.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Destroy(GameObject.Find("Head1"));
    }
}
