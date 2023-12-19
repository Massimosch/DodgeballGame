using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    public float throwForce = 10.0f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void PickUp(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
        rb.velocity = Vector2.zero;
    }

    public void Throw(Vector2 direction)
    {
    transform.SetParent(null);
    rb.AddForce(Vector2.right * throwForce, ForceMode2D.Impulse);
    }
}