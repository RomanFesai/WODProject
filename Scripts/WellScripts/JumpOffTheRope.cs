using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOffTheRope : MonoBehaviour
{
    public Rigidbody2D PlayerRigidBody;
    [SerializeField] private bool playerInRange = false;
    [SerializeField] private GameObject ActionKeyHint = default;
   
    // Update is called once per frame
    void Update()
    {
        if (playerInRange == true && Input.GetKeyDown(KeyCode.X))
        {
            PlayerRigidBody.constraints = RigidbodyConstraints2D.None;
            PlayerRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            Destroy(GameObject.Find("InvisibleWall"));
            PlayerRigidBody.gravityScale = 50f;
            playerClimbRope.canClimb = false;
            playerClimbRope.MovementSpeed = 6f;
            GameObject.Find("_Player").GetComponent<Animator>().enabled = true;
            ActionKeyHint.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            ActionKeyHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            ActionKeyHint.SetActive(false);
        }
    }
}
