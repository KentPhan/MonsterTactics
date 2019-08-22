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
        Square sq = GridSystem.Instance.theMap[transform.localPosition + Vector3.forward];
        if (sq)
            neighbors.Add(sq);

        sq = GridSystem.Instance.theMap[transform.localPosition + Vector3.back];
        if (sq)
            neighbors.Add(sq);

        sq = GridSystem.Instance.theMap[transform.localPosition + Vector3.left];
        if (sq)
            neighbors.Add(sq);

        sq = GridSystem.Instance.theMap[transform.localPosition + Vector3.right];
        if (sq)
            neighbors.Add(sq);
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

        foreach (Square square in neighbors)
            square.Range(range - 1);
    }

    public void Clear()
    {
    }

}
