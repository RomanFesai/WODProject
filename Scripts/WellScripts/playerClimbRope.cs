using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClimbRope : MonoBehaviour
{
    public Rigidbody2D rb;
    public static float MovementSpeed = 2f;
    private Vector3 movement;
    public static bool canClimb = true;
    public Animator PlayerAnim;
    // Start is called before the first frame update
    void Start()
    {
        canClimb = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnim = GameObject.Find("_Player").GetComponent<Animator>();
        movement = Vector3.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        Climb(canClimb);
        if (movement != Vector3.zero)
        {
            PlayerAnim.SetFloat("Horizontal", movement.x);
            PlayerAnim.SetFloat("Vertical", movement.y);
            PlayerAnim.SetFloat("Speed", movement.sqrMagnitude);
            PlayerAnim.SetBool("moving", true);
        }
        else
        {
            PlayerAnim.SetBool("moving", false);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * MovementSpeed * Time.deltaTime);
    }

    public void Climb(bool canClimb)
    {
        if (canClimb)
        {
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
        }
    }
}
