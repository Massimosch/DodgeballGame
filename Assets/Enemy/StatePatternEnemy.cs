using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemy : MonoBehaviour
{
    public float gameStartDuration;
    public float gameStartMovementSpeed;
    public Transform[] waypoints;
    public Transform ballHolder;
    public MeshRenderer indicator;

    [HideInInspector] public Transform currentTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public GameStartState gameStartState;

    [HideInInspector] public NavMeshAgent navMeshAgent;

    void Awake()
    {
        gameStartState = new GameStartState(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = gameStartState;
    }

    void Update()
    {
        currentState.UpdateState();
    }
}
