using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    // Start is called before the first frame update
    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 InitialPosition;

    public PlayerMovement GR;

    void Start()
    {
        InitialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GR.gunSwayReady==true) {
            float movementX = -Input.GetAxis("Mouse X") * amount;
            float movementY = -Input.GetAxis("Mouse Y") * amount;
            movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
            movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

            Vector3 finalPosition = new Vector3(movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + InitialPosition, Time.deltaTime * smoothAmount); 
        }
    }
}
