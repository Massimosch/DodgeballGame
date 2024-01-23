using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLine : MonoBehaviour
{
    [SerializeField] private GameObject EnemyOutOfGamePos;
    [SerializeField] private GameObject PlayerOutOfGamePos;
    public Player player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Die();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.transform.position = EnemyOutOfGamePos.transform.position;
        }
    }
}