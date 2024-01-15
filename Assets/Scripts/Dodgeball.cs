using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    private Rigidbody2D rb;
    private DodgeballDamage damageScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageScript = GetComponent<DodgeballDamage>();
        damageScript.enabled = false;
    }

    public void Throw(Vector2 direction, float force, GameObject thrower)
    {
        damageScript.SetThrower(thrower);
        damageScript.enabled = true;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            damageScript.HandleCollision(collision.gameObject);
        }
    }
}

