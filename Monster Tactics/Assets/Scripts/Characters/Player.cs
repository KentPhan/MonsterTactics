using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum PlayerActions
{
    ATTACK
}

public class Player : MonoBehaviour
{
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(GridSystem.Instance.clickedGridPosition);
    }

    public void Move(Vector3 destionation)
    {
        agent.SetDestination(destionation);
    }
}
