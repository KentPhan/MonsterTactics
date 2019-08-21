using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Just because this is only a proto-type, I just obly make 10 inventory slots.
    private int maxSlot = 10;
    private Item[] slots;    

    private static InventorySystem _instance;
    public static InventorySystem Instance { get { return _instance; } }

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

    private void Start()
    {
        // Just because this is only a proto-type, I just obly make 10 inventory slots.
        slots = new Item[maxSlot];

        // Initialize with empty slots
        for(int i = 0; i < maxSlot; i++)
        {
            slots[i] = new NoItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(Item item)
    {

    }

    public void Remove()
    {

    }
}
