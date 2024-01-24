using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLine : MonoBehaviour
{
    [SerializeField] private GameObject EnemyOutOfGamePos;
    [SerializeField] private GameObject PlayerOutOfGamePos;
    [SerializeField] public bool isLineDeadly = false;
    public Player player;

    public void SetLineDeadly(bool isDeadly)
    {
        isLineDeadly = isDeadly;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isLineDeadly)
        {
            player.Die();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.transform.position = EnemyOutOfGamePos.transform.position;
        }
    }
}