using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] protected Color none = Color.red, over = Color.yellow, selected = Color.green;
    
    //Squares that are adjacent to this square
    List<Square> neighbors = new List<Square>();
    [SerializeField] int range;

    protected LineRenderer lineRenderer;
    GridSystem gridSystem;

    private void Awake()
    {
        gridSystem = GetComponentInParent<GridSystem>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material.color = none;
    }

    private void Start()
    {
        //Maps the neighboring squares
        foreach (Square square in gridSystem.squares)
            if (Vector3.Distance(transform.localPosition, square.transform.localPosition) <= 1.1f)
                if (square != this)
                    neighbors.Add(square);
    }

    private void OnMouseEnter()
    {
        gridSystem.mousePosition = transform.position;
        Hover(0);
    }

    private void OnMouseDown()
    {
        Select(range);
    }

    private void OnMouseExit()
    {
        Leave(range);
    }

    protected void Hover(int jumps)
    {
        lineRenderer.material.color = over;
        if (jumps > 0)
            foreach (Square neighbor in neighbors)
                if(neighbor.lineRenderer.material.color == neighbor.none)
                    neighbor.Hover(jumps - 1);
    }

    protected void Select(int jumps)
    {
        lineRenderer.material.color = selected;
        if (jumps > 0)
            foreach (Square neighbor in neighbors)
                if(neighbor.lineRenderer.material.color == neighbor.none)
                    neighbor.Hover(jumps - 1);
    }


    protected void Leave(int jumps)
    {
        lineRenderer.material.color = none;
        if (jumps > 0)
            foreach (Square neighbor in neighbors)
                if(neighbor.lineRenderer.material.color != neighbor.none)
                    neighbor.Leave(jumps - 1);
    }

}
