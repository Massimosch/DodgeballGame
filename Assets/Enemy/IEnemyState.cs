using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void UpdateState();

    void OnTriggerEnter(Collider other);

    void OnGameStartState();

    void OnGameOnState();

}
