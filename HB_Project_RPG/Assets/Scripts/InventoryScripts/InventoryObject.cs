using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InterfaceType
{
    Inventory,
    Equipment,
    QuickSlot,
    Skill,
    Box,
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventoty")]
public class InventoryObject : ScriptableObject
{
    public List<ItemObjectDatabase> databases = new List<ItemObjectDatabase>();
    public InterfaceType type;

    // 실질적인 인벤토리
    [SerializeField]
    private Inventory container = new Inventory();

    public InventorySlot[] Slots => container.slots;

    // 비어있는 슬롯 개수를 가져오는 함수
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;

            foreach (InventorySlot slot in Slots)
            {
                if (slot.item.id < 0)
                    counter++;
            }

            return counter;
        }
    }

    // 중복되는 아이템을 더해주는 함수
    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
            return false;

        // 동일한 아이템의 슬롯이 있는지 검색
        InventorySlot slot = FindItemInInventory(item);

        foreach (ItemObjectDatabase db in databases)
        {
            if (!db.itemObjects[item.id].stackable || slot == null)
                GetEmptySlot().AddItem(item, amount);
            else
                slot.AddAmount(amount);
        }

        return true;
    }

    // 인벤토리에 동일한 아이템의 슬롯이 있는지 검색하는 함수
    public InventorySlot FindItemInInventory(Item item)
    {
        return Slots.FirstOrDefault(i => i.item.id == item.id);
    }

    public InventorySlot GetEmptySlot()
    {
        return Slots.FirstOrDefault(i => i.item.id < 0);
    }

    public bool IsContainItem(ItemObject itemObject)
    {
        return Slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
    }

    public void SwapItems(InventorySlot itemSlotA, InventorySlot itemSlotB)
    {
        if (itemSlotA == itemSlotB)
            return;

        if (itemSlotB.CanPlaceInSlot(itemSlotA.itemObject) &&
            itemSlotA.CanPlaceInSlot(itemSlotB.itemObject))
        {
            InventorySlot temp = new InventorySlot(itemSlotB.item, itemSlotB.amount);
            itemSlotB.UpdateSlot(itemSlotA.item, itemSlotA.amount);
            itemSlotA.UpdateSlot(temp.item, temp.amount);
        }
    }

    public void Clear()
    {
        container.Clear();
    }
}
