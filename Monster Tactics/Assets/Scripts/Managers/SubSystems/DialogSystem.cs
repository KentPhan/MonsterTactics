using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Constants;

public class DialogSystem : MonoBehaviour
{
    private int rayCastMask = 0;
    [SerializeField]
    private Camera camera;

    private bool uiEnable = false;
    private DialogUI ui;

    void Start()
    {
        rayCastMask = LayerMask.GetMask(Layers.GRID);
        ui = GetComponent<DialogUI>();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 500, rayCastMask))
            {
                ui.parseAction();
                if (!ui.checkIfInsideGUI(Input.mousePosition.x, Screen.height - Input.mousePosition.y))
                {
                    ui.topleftX = Input.mousePosition.x;
                    ui.topleftY = Screen.height - Input.mousePosition.y;
                    ui.topleftXOld = Input.mousePosition.x;
                    ui.topleftYOld = Screen.height - Input.mousePosition.y;
                    ui.enabled = !ui.enabled;
                }
            }
        }
    }


}
