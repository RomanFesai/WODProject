using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QT_HandsTied : MonoBehaviour
{

    private float _fillAmount = 0;
    private float _fadeAmount = 0;
    private bool Tied = true;
    public bool EscapeTry = false;
    public Animator HandsTied;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("_Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Tied) Player.SetActive(false);
        else Player.SetActive(true);

        if (Input.GetKeyDown(KeyCode.X) && Tied == true)
        {
            EscapeTry = true;
            _fillAmount += .2f;
            HandsTied.SetBool("Try", true);
        }
        else
        {
            EscapeTry = false;
            HandsTied.SetBool("Try", false);
        }
        
        if(_fillAmount > 1)
        {
            HandsTied.SetFloat("Escaped", _fillAmount);
            Tied = false;
            _fillAmount = 0;
        }

        if (Tied == false)
        {
            Invoke("DestroyQT", 1f);
        }

        _fadeAmount += Time.deltaTime;
        if(_fadeAmount > .05)
        {
            _fadeAmount = 0;
            _fillAmount -= .02f;
        }
        if(_fillAmount < 0)
        {
            _fillAmount = 0;
        }

        GetComponent<Image>().fillAmount = _fillAmount;
    }


    void DestroyQT()
    {
        Destroy(GameObject.Find("Escape"));
    }
}
