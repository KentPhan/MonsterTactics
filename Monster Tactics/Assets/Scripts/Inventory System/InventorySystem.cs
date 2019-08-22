using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Just because this is only a proto-type, I just obly make 10 inventory slots.
    private int maxSlot = 10;
    private GameObject[] slots;
    //private Item[] itemTypes;

    // Getter, Setter
    public int MaxSlot { get { return maxSlot; } }
    public GameObject[] Slots { get { return slots; } }

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

        // Just because this is only a proto-type, I just obly make 10 inventory slots.
        slots = new GameObject[MaxSlot];

        // Initialize with empty slots
        for (int i = 0; i < maxSlot; i++)
        {
            slots[i] = new GameObject();
            slots[i].AddComponent<NoItem>();
            slots[i].transform.SetParent(gameObject.transform);
        }
    }

    private void Start()
    {
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
