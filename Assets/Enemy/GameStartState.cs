using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class GameStartState : IEnemyState
{
    private StatePatternEnemy enemy;

    private int nextWaypoint;

    public GameStartState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }
    public void UpdateState()
    {
        Patrol();
    }


    public void OnGameOnState()
    {
        
    }

    public void OnGameStartState()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    void Patrol()
    {
        enemy.indicator.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.waypoints[nextWaypoint].position;
        enemy.navMeshAgent.isStopped = false;

        // Vaihdetaan waypointia kun päästään nykyiseen. Navmeshissä on työkalut

        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % enemy.waypoints.Length; //Looppailee läpi
        }
    }
}