using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotList : MonoBehaviour
{
    // For future implementation
    //public SlotObjectPool SlotPool;

    public GameObject ItemSlotButton;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < InventorySystem.Instance.MaxSlot; i++)
        {
            //Item item = InventorySystem.Instance.Slots[i];
            GameObject slot = InventorySystem.Instance.Slots[i];
            Item item = slot.GetComponent<Item>();

            GameObject itemSlotButton = Instantiate(ItemSlotButton);
            itemSlotButton.transform.SetParent(gameObject.transform);

            ItemSlot itemSlot = itemSlotButton.GetComponent<ItemSlot>();
            itemSlot.Setup(item);
        }
    }
}
