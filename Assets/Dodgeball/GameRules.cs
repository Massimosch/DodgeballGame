using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public GameObject gameFloor;
    public GameObject middleLine;
    public GameObject playerOutOfGamePos;
    public GameObject enemyOutOfGamePos;

    public Dodgeball dodgeball;




    public float yAxisThreshold;

    void Start()
    {
        gameFloor = GameObject.Find("GameFloor");
        middleLine = GameObject.Find("MiddleLine");
        playerOutOfGamePos = GameObject.Find("PlayerOutOfGamePos");
        enemyOutOfGamePos = GameObject.Find("EnemyOutOfGamePos");

        yAxisThreshold = 0;
    }

    void Update()
    {

    }

    void OnPlayerCrossThreshold(GameObject player)
    {
        player.transform.position = playerOutOfGamePos.transform.position;
    }

    void OnEnemyCrossThreshold(GameObject enemy)
    {
        enemy.transform.position = enemyOutOfGamePos.transform.position;
    }

    void EliminatePlayer(GameObject player)
    {
        player.transform.position = playerOutOfGamePos.transform.position;
    }

    void EliminateEnemy(GameObject enemy)
    {
        enemy.transform.position = enemyOutOfGamePos.transform.position;
    }
}