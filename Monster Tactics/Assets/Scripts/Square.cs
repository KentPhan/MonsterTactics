using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] protected Color none = Color.clear, peek = Color.cyan, traversible = Color.yellow, select = Color.green, ping = Color.red;

    List<Square> neighbors = new List<Square>();

    [SerializeField] protected MeshRenderer meshRenderer;
    protected LineRenderer lineRenderer;
    protected int steps = -1;

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
            if (Input.GetMouseButton(1))
            {
                meshRenderer.material.color = ping;
            }
            else
            {
                meshRenderer.material.color = peek;
            }
        }
        else if (meshRenderer.material.color == traversible)
        {
            GridSystem.Instance.hoveringSquare = this;
            meshRenderer.material.color = select;
        }
    }

    private void OnMouseDown()
    {
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
        steps = range;
        meshRenderer.material.color = traversible;
        if (range > 0)
        {
            range--;
            foreach (Square square in neighbors)
                if (range > square.steps)
                    square.Range(range);
        }
    }

    public bool IsInRange()
    {
        return meshRenderer.material.color == select;
    }

    public void Clear()
    {
        steps = -1;
        meshRenderer.material.color = none;
        foreach (Square square in neighbors)
            if (square.meshRenderer.material.color == traversible || square.meshRenderer.material.color == select)
                square.Clear();
    }

}
