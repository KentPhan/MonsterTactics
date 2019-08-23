using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.AI;


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
        if (GridSystem.Instance.clickedSquare)
            agent.SetDestination(GridSystem.Instance.clickedSquare.transform.position);
    }

    private void OnMouseDown()
    {
        if (GridSystem.Instance.clickedSquare)
            GridSystem.Instance.clickedSquare.Range(range);
    }

    public void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
