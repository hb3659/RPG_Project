using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];

    // 해당 슬롯을 소유하고 있는 부모
    [NonSerialized]
    public InventoryObject parent;
    // 슬롯을 표시하는 ui 게임 오브젝트
    [NonSerialized]
    public GameObject slotUI;

    // 슬롯에 아이템이 갱신될 때 부가적인 처리를 위함
    [NonSerialized]
    public Action<InventorySlot> OnPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> OnPostUpdate;

    // 실제 해당 슬롯에 포함되어 있는 아이템
    public Item item;
    // stackable 인 아이템의 개수를 표시하기 위함
    public int amount;

    // 슬롯에 포함되어 있는 아이템의 데이터를 가져오는 프로퍼티
    public ItemObject itemObject
    {
        get
        {
            // id == -1 이면 아이템이 존재하지 않는다.
            // 만약 아이템이 존재한다면 ItemObject 를 반환
            foreach(ItemObjectDatabase db in parent.databases)
            {
                return item.id >= 0 ? db.itemObjects[item.id] : null;
            }

            return null;
        }
    }

    // 기본 생성자
    // 해당 슬롯에 새로운 아이템을 생성
    public InventorySlot() => UpdateSlot(new Item(), 0);
    // 아이템이 들어있는 슬롯을 생성하는 경우
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);

    public void AddItem(Item item, int amount) => UpdateSlot(item, amount);

    // 슬롯에서 아이템을 삭제
    public void RemoveItem() => UpdateSlot(new Item(), 0);

    // 슬롯에 있는 아이템의 개수를 늘리는 경우
    public void AddAmount(int value) => UpdateSlot(item, amount += value);

    // 드래그 앤 드롭 시 호출되는 함수
    public void UpdateSlot(Item item, int amount)
    {
        // 기존에 슬롯에 있던 아이템을 처리하는 액션
        OnPreUpdate?.Invoke(this);

        this.item = item;
        this.amount = amount;

        // 새롭게 설정된 아이템에 대해서 어떠한 부가적인 행동을 할지
        OnPostUpdate?.Invoke(this);
    }

    // 아이템이 슬롯에 지정될 수 있는지 판단하는 함수
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        // alloweditems 가 지정되지 않았거나
        if (allowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            return true;

        foreach(ItemType type in allowedItems)
        {
            if (itemObject.type == type)
                return true;
        }

        return false;
    }
}
