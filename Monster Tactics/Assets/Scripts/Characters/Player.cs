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
    [SerializeField] int range = 3;

    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GridSystem.Instance.clickedGridPosition)
            agent.SetDestination(GridSystem.Instance.clickedGridPosition.transform.position);
    }

    private void OnMouseDown()
    {
        if (GridSystem.Instance.clickedGridPosition)
            GridSystem.Instance.clickedGridPosition.Range(range);
    }

    public void Move(Vector3 destionation)
    {
        agent.SetDestination(destionation);
    }
}
