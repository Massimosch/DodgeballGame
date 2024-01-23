using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{
    public Player player;
    public MiddleLine middleLine;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player.HasDodgeball) // Access HasDodgeball like a field
        {
            middleLine.SetLineDeadly(true); // Make the middle line deadly
        }
    }
}

