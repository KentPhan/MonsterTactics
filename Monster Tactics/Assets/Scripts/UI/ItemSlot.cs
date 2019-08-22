using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Button slot;
    private Text itemName;
    private Image itemImage;

    // Start is called before the first frame update
    void Start()
    {
        slot = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Item item)
    {
        //itemImage.sprite = item.Icon;
        //itemName.text = item.name;
    }
}
