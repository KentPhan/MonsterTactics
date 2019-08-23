using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private static GridSystem _instance;
    public static GridSystem Instance { get { return _instance; } }
    [SerializeField] public Square startSquare;

    // Added last clicked and last hovered position
    [System.NonSerialized] public Square hoveringSquare;
    [System.NonSerialized] public Square clickedSquare;
    //[System.NonSerialized] public List<Square> squares = new List<Square>();
    public Dictionary<Vector3, Square> theMap = new Dictionary<Vector3, Square>();

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
        clickedSquare = startSquare;
        //Creates a master list of all squares in this grid system
        foreach (Square square in GetComponentsInChildren<Square>())
        {
            theMap.Add(square.transform.localPosition, square);
            //squares.Add(square);
        }
    }

}
