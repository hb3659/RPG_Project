using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    // 인벤토리를 모두 비우는 함수
    public void Clear()
    {
        foreach(InventorySlot slot in slots)
        {
            slot.RemoveItem();
        }
    }

    // 아이템이 해당 인벤토리에 포함되어 있는지 식별하는 함수
    public bool IsContain(ItemObject itemObject)
    {
        //return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
        return IsContain(itemObject.data.id);
    }

    public bool IsContain(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
}
