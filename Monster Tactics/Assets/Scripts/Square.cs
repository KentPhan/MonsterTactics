using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    protected Color none = Color.clear, peek = Color.cyan, traversible = Color.yellow, select = Color.green;
    
    List<Square> neighbors = new List<Square>();

    [SerializeField] protected MeshRenderer meshRenderer;
    protected LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshRenderer.material.color = Color.clear;
    }

    private void Start()
    {
        //Maps the neighboring squares
        foreach (Square square in GridSystem.Instance.squares)
            if (Vector3.Distance(transform.localPosition, square.transform.localPosition) <= 1.1f)
                if (square != this)
                    neighbors.Add(square);
    }

    private void OnMouseEnter()
    {
        if (meshRenderer.material.color == none)
        {
            GridSystem.Instance.enteringGridPosition = this;
            meshRenderer.material.color = peek;
        }
        else if(meshRenderer.material.color == traversible)
        {
            GridSystem.Instance.enteringGridPosition = this;
            meshRenderer.material.color = select;
        }
    }

    private void OnMouseDown()
    {
        if (meshRenderer.material.color == select)
        {
            meshRenderer.material.color = none;
            GridSystem.Instance.clickedGridPosition.Clear();
            GridSystem.Instance.clickedGridPosition = this;
        }
    }

    private void OnMouseExit()
    {
        if (meshRenderer.material.color == peek)
            meshRenderer.material.color = none;
        else if (meshRenderer.material.color == select)
            meshRenderer.material.color = traversible;
    }
    
    public void Range(int range)
    {
        meshRenderer.material.color = traversible;
        if(range > 0)
            foreach (Square square in neighbors)
                if (square.meshRenderer.material.color == none)
                    square.Range(range - 1);
    }

    public void Clear()
    {
        meshRenderer.material.color = none;
        foreach (Square square in neighbors)
            if (square.meshRenderer.material.color == traversible)
                square.Clear();
    }

}
