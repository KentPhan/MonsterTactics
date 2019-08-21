using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    private Button slot;
    public Text ItemName;
    public Image ItemImage;

    // Start is called before the first frame update
    void Start()
    {
        slot = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public 
}
