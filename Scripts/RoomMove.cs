using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMove : MonoBehaviour
{

    public Vector2 CamChange;
    public Vector3 playerChange;
    private CameraBehaviour cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraBehaviour>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cam.minPosition += CamChange;
            cam.maxPosition += CamChange;
            other.transform.position += playerChange;
        } 
    }
}
