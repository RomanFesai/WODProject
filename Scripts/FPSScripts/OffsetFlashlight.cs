using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlashlight : MonoBehaviour
{
    private Vector3 vectorOffset;
    private GameObject goFollow;
    [SerializeField] private float speed = 3.0f;

    private void Start()
    {
        goFollow = Camera.main.gameObject;
        vectorOffset = transform.position - goFollow.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, goFollow.transform.position + vectorOffset, speed * Time.deltaTime);
        //transform.position = goFollow.transform.position + vectorOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
    }
}
