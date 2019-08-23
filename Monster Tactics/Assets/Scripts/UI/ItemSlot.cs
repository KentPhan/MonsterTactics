using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [HideInInspector]
    public Item item;

    private Button slot;
    private Text itemName;
    private Image itemImage;

    private GameObject itemSlotContainer;
    private ItemSlotList slotList;

    // Start is called before the first frame update
    void Start()
    {
        itemSlotContainer = transform.parent.gameObject;
        slotList = itemSlotContainer.GetComponent<ItemSlotList>();

        slot = GetComponent<Button>();
        slot.onClick.AddListener(ButtonClick);
    }

    public void Setup()
    {
        //itemImage.sprite = item.Icon;
        //itemName.text = item.name;
    }

    public void ButtonClick()
    {
        item.Equip();
        Destroy(item.transform.gameObject);
        Destroy(gameObject);
    }
}
