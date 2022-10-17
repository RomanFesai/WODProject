using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCharMercyEvent : MonoBehaviour
{
    public GameObject HeadEvent;
    public GameObject Player;
    public static bool have_well_key = false;

    private void Start()
    {
        
    }
    void Update()
    {
        string answer = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("answer")).value;
        if(answer == "Kill")
        {
            Player.SetActive(false);
            have_well_key = true;
            HeadEvent.SetActive(true);
            GetComponent<KeyCharMercyEvent>().enabled =false;
        }
    }
}
