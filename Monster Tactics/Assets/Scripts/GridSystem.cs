using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private static GridSystem _instance;
    public static GridSystem Instance { get { return _instance; } }
    [SerializeField] public Square startGridPosition;

    // Added last clicked and last hovered position
    [System.NonSerialized] public Square enteringGridPosition;
    [System.NonSerialized] public Square clickedGridPosition;
    [System.NonSerialized] public List<Square> squares = new List<Square>();

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        clickedGridPosition = startGridPosition;
        //Creates a master list of all squares in this grid system
        foreach (Square square in GetComponentsInChildren<Square>())
            squares.Add(square);
    }

}
