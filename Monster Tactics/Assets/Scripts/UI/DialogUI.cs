using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Constants;

public class DialogUI : MonoBehaviour
{
    //public enum Action {Move,Pick,Fight};
    private int maxActionNum = 4;
    private bool[] actionListTable;
    private List<Actions> ActionList;


    [HideInInspector]
    public float topleftX;
    [HideInInspector]
    public float topleftY;

    [HideInInspector]
    public float topleftXOld;
    public float topleftYOld;

    private int buttonWidth = 200;
    private int buttonHeight = 50;

    public delegate void EventHandler(Actions action);
    public static event EventHandler ActionButtonClicked = new EventHandler((actionL) => { });

    private void Awake()
    {
        actionListTable = new bool[maxActionNum];
        ActionList = new List<Actions>();

        for (int i = 0; i < maxActionNum; i++)
        {
            actionListTable[i] = false;
        }

        // Place holder
        ActionList.Add(Actions.Attack);
    }

    public void UpdateActionList(List<Actions> actions)
    {
        ActionList = actions;
    }

    public void parseAction()
    {
        for (int i = 0; i < maxActionNum; i++)
        {
            actionListTable[i] = false;
        }
        foreach (Actions action in ActionList)
        {
            actionListTable[(int)action] = true;
        }
    }

    public bool checkIfInsideGUI(float x, float y)
    {
        if (x > topleftXOld && x < topleftXOld + buttonWidth)
        {
            if (y > topleftYOld && y < ActionList.Count * buttonHeight + topleftYOld)
            {
                return true;
            }
        }
        return false;
    }

    void OnGUI()
    {
        int nextbutton = 0;
        for (int i = 0; i < maxActionNum; i++)
        {
            if (actionListTable[i])
            {
                if (GUI.Button(new Rect(topleftX, topleftY + nextbutton * buttonHeight, buttonWidth, buttonHeight), ((Actions)i).ToString()))
                {
                    ActionButtonClicked.Invoke((Actions)i);
                    this.enabled = false;
                }
                nextbutton++;
            }
        }
    }
}
