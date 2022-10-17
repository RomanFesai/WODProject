using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerMovement : MonoBehaviour
{

    public float MovementSpeed = 2f;
    public Animator animator;

    public Rigidbody2D rb;

    private Vector3 movement;

    [Header("FootStep Parametrs")]
    [SerializeField] private float baseStepSpeed = 0.2f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] GrassStepClips = default;
    public float footStepTimer = 0;

    public static bool isWalking = false;

    private void Start()
    {
        animator = GameObject.Find("_Player").GetComponent<Animator>();
    }
    void Update()
    {
        movement = Vector3.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement != Vector3.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetBool("moving", true);
            isWalking = true;
        }
        else
        {
            animator.SetBool("moving", false);
            isWalking = false;
        }

        footStepTimer -= Time.deltaTime;
        if (footStepTimer <= 0 && isWalking == true)
        {
            footstepAudioSource.PlayOneShot(GrassStepClips[UnityEngine.Random.Range(0, GrassStepClips.Length - 1)]);
            footStepTimer = baseStepSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {   
            animator.enabled = false;
            return;
        }
        else
        {
            animator.enabled = true;
        }

        rb.MovePosition(transform.position + movement * MovementSpeed * Time.deltaTime);

        /*if(Input.GetKey(KeyCode.LeftShift))
        {
            MovementSpeed = 5f;
        }
        else
        {
            MovementSpeed = 2f;
        }*/
    }
}
