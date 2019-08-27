using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] protected Color none = Color.clear, peek = Color.cyan, traversible = Color.yellow, select = Color.green;

    List<Square> neighbors = new List<Square>();

    [SerializeField] protected MeshRenderer meshRenderer;
    protected LineRenderer lineRenderer;
    protected int steps = int.MaxValue;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshRenderer.material.color = Color.clear;
    }

    private void Start()
    {
        if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.forward))
            neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.forward]);
        if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.back))
            neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.back]);
        if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.left))
            neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.left]);
        if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.right))
            neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.right]);
    }

    private void OnMouseEnter()
    {
        if (meshRenderer.material.color == none)
        {
            GridSystem.Instance.hoveringSquare = this;
            meshRenderer.material.color = peek;
        }
        else if (meshRenderer.material.color == traversible)
        {
            GridSystem.Instance.hoveringSquare = this;
            meshRenderer.material.color = select;
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            return;
        }
        else if (Input.GetMouseButtonDown(1))
            return;
    }

    private void OnMouseExit()
    {
        if (meshRenderer.material.color == peek)
            meshRenderer.material.color = none;
        else if (meshRenderer.material.color == select)
            meshRenderer.material.color = traversible;
    }

    public void Range(int range, int isteps = 0)
    {
        steps = isteps;
        meshRenderer.material.color = traversible;
        if (range > 0)
        {
            foreach (Square square in neighbors)
            {
                if ((steps + 1) < (square.steps))
                    square.Range(range - 1, steps + 1);
            }
        }
    }

    public bool IsInRange()
    {
        return meshRenderer.material.color == select;
    }

    public int ActionPointCost(int range)
    {
        return steps;
    }

    public void Clear()
    {
        steps = int.MaxValue;
        meshRenderer.material.color = none;
        foreach (Square square in neighbors)
            if (square.meshRenderer.material.color == traversible || square.meshRenderer.material.color == select)
                square.Clear();
    }

}
