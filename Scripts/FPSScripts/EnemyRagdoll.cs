using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    public float health = 5f;
    public GameObject AtkHandDisable;
    void Start()
    {
        setRigidBodyState(true);
        setColliderState(false);
    }
    
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            die();
        }
    }
    public void die()
    {
        GetComponent<Animator>().enabled = false;
        setRigidBodyState(false);
        setColliderState(true);
        GetComponent<EnemyAi>().enabled = false;
        AtkHandDisable.GetComponent<BoxCollider>().enabled = false;
        FindObjectOfType<AudioManager>().Play("MonsterDeath");
    }

    void setRigidBodyState(bool state) {

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider collider in colliders)
        {
            collider.enabled = state;
        }
        GetComponent<Collider>().enabled = !state;
    }
}
