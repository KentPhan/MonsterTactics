using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
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

    protected LineRenderer rootRenderer;

    void Awake()
    {
        if (_instance != null && _instance != this)
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

        // For rootRenderer
        rootRenderer = GetComponent<LineRenderer>();
        resetRootRenderer();
    }

    public void highlightRoot(Square startSquare,Square endSquare)
    {
        Vector3 startposition = new Vector3(startSquare.transform.position.x, 0.5f, startSquare.transform.position.z);
        Vector3 endposition = new Vector3(endSquare.transform.position.x, 0.5f, endSquare.transform.position.z);
        int numberofpoints = rootRenderer.positionCount;
        if(!isRendererDrawing())
        {
            rootRenderer.positionCount = numberofpoints + 1;
            rootRenderer.SetPosition(numberofpoints - 1, startposition);
        }
        else
        {
            rootRenderer.positionCount = numberofpoints + 1;
        }
        rootRenderer.SetPosition(numberofpoints, endposition);
    }

    public void resetRootRenderer()
    {
        rootRenderer.positionCount = 1;
    }

    public bool isRendererDrawing()
    {
        if(rootRenderer.positionCount == 1)
        {
            return false;
        }
        return true;
    }
}
