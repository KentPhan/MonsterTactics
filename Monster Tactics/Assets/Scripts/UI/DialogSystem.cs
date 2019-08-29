using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Constants;

public class DialogSystem : MonoBehaviour
{
    private static DialogSystem _instance;
    public static DialogSystem Instance
    {
        get { return _instance; }
    }
    
    private DialogUI ui;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        ui = GetComponent<DialogUI>();
    }

    public void TurnOnDialog(float x, float y)
    {
        ui.parseAction();
        if( !ui.enabled ||  !ui.checkIfInsideGUI(x, Screen.height - y))
        {
            ui.topleftX = x;
            ui.topleftY = Screen.height - y;
            ui.topleftXOld = x;
            ui.topleftYOld = Screen.height - y;
            ui.enabled = true;
        }
    }

    public void TurnOffDialog()
    {
        ui.enabled = false;
    }

    public void SendActionList(List<Actions> actions)
    {
        ui.UpdateActionList(actions);
    }


}
