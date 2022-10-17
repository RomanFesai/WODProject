using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;
    public static bool FlashlightActive = false;
    // Start is called before the first frame update
    void Start()
    {
        //flashlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!FlashlightActive)
            {
                flashlight.SetActive(true);
                FlashlightActive = true;
                FindObjectOfType<AudioManager>().Play("FlashLightClick");
            }
            else if (FlashlightActive)
            {
                flashlight.SetActive(false);
                FlashlightActive = false;
                FindObjectOfType<AudioManager>().Play("FlashLightClick");
            }
        }
    }
}
