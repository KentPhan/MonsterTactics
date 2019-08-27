using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    public enum Action {Move,Pick,Fight};
    private int maxActionNum = 3;
    private bool[] actionListTable;

    [HideInInspector]
    public List<Action> ActionList;
    

    [HideInInspector]
    public float topleftX;
    [HideInInspector]
    public float topleftY;

    [HideInInspector]
    public float topleftXOld;
    public float topleftYOld;

    private int buttonWidth = 200;
    private int buttonHeight = 50;

    public delegate void EventHandler(Action action);
    public static event EventHandler ActionButtonClicked = new EventHandler((actionL)=> { });

    private void Awake()
    {
        actionListTable = new bool[maxActionNum];
        ActionList = new List<Action>();
        ActionButtonClicked += testSubscriber;

        for (int i = 0; i < maxActionNum; i++)
        {
            actionListTable[i] = false;
        }

        ActionList.Add(Action.Fight);
        ActionList.Add(Action.Move);
        ActionList.Add(Action.Fight);
    }

    public void parseAction()
    {
        for(int i = 0; i < maxActionNum; i++)
        {
            actionListTable[i] = false;
        }
        foreach(Action action in ActionList)
        {
            actionListTable[(int)action] = true;
        }
    }

    public bool checkIfInsideGUI(float x, float y)
    {
        if( x > topleftXOld && x < topleftXOld + buttonWidth)
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
        for(int i = 0;i < maxActionNum; i++)
        {
            if (actionListTable[i])
            {
                if (GUI.Button(new Rect(topleftX, topleftY + nextbutton * buttonHeight, buttonWidth, buttonHeight), ((Action)i).ToString()))
                {
                    ActionButtonClicked.Invoke((Action)i);
                }
                nextbutton++;
            }
        }
    }

    public void testSubscriber(Action action)
    {
        Debug.Log("The action which is taken is " + action);
    }
}
