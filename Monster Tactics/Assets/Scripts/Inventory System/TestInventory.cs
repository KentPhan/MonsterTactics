using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            gameObject.AddComponent<Assets.Scripts.Inventory_System.Items.AxeItem>();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(GetComponent<Assets.Scripts.Inventory_System.Items.AbstractItem>());
        }
    }
}
