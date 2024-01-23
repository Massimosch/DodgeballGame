using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    private DodgeballDamage damageScript;
    [SerializeField] Rigidbody rb;
    [SerializeField] public bool thrownByPlayer = false;
    [SerializeField] public bool thrownByEnemy = false;

    void Start()
    {
        damageScript = GetComponent<DodgeballDamage>();
        damageScript.enabled = false;

    }

    public void Throw(Vector3 direction, float force, GameObject thrower)
    {
        if (thrower.CompareTag("Player"))
        {
            thrownByPlayer = true;
            thrownByEnemy = false;
        }
        else if (thrower.CompareTag("Enemy"))
        {
            thrownByPlayer = false;
            thrownByEnemy = true;
        }

        damageScript.SetThrower(thrower);
        damageScript.enabled = true;
        rb.AddForce(direction * force, ForceMode.VelocityChange);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            damageScript.HandleCollision(collision.gameObject);
        }
    }
}

