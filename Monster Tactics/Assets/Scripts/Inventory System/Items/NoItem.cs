using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoItem : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Equip()
    {
        Debug.Log("No Item is used");
    }
}
