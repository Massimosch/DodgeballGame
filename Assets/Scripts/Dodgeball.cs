using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    public float throwForce = 10.0f;
    private Rigidbody2D rb;
    private bool isHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void PickUp(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
        rb.velocity = Vector2.zero;
        isHeld = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    Debug.Log("Dodgeball collided with: " + collision.gameObject.name);
    // check if the dodgeball is not being held (has been thrown)
    if (!IsHeld())
    {
        // check if the dodgeball has collided with an enemy after throw
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            rb.velocity = Vector2.zero;
        }
    }
}

    public void Throw(Vector2 direction)
    {
        transform.SetParent(null);
        rb.AddForce(Vector2.right * throwForce, ForceMode2D.Impulse);
        isHeld = false;
    }

    public bool IsHeld()
    {
        return isHeld;
    }
}