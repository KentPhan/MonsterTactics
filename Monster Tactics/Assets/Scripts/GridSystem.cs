using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [System.NonSerialized] public Vector3 mousePosition;
    [System.NonSerialized] public List<Square> squares = new List<Square>();

    void Awake()
    {
        //Creates a master list of all squares in this grid system
        foreach (Square square in GetComponentsInChildren<Square>())
            squares.Add(square);
    }

}
