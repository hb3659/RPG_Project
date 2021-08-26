using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];

    // �ش� ������ �����ϰ� �ִ� �θ�
    [NonSerialized]
    public InventoryObject parent;
    // ������ ǥ���ϴ� ui ���� ������Ʈ
    [NonSerialized]
    public GameObject slotUI;

    // ���Կ� �������� ���ŵ� �� �ΰ����� ó���� ����
    [NonSerialized]
    public Action<InventorySlot> OnPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> OnPostUpdate;

    // ���� �ش� ���Կ� ���ԵǾ� �ִ� ������
    public Item item;
    // stackable �� �������� ������ ǥ���ϱ� ����
    public int amount;

    // ���Կ� ���ԵǾ� �ִ� �������� �����͸� �������� ������Ƽ
    public ItemObject itemObject
    {
        get
        {
            // id == -1 �̸� �������� �������� �ʴ´�.
            // ���� �������� �����Ѵٸ� ItemObject �� ��ȯ
            foreach(ItemObjectDatabase db in parent.databases)
            {
                return item.id >= 0 ? db.itemObjects[item.id] : null;
            }

            return null;
        }
    }

    // �⺻ ������
    // �ش� ���Կ� ���ο� �������� ����
    public InventorySlot() => UpdateSlot(new Item(), 0);
    // �������� ����ִ� ������ �����ϴ� ���
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);

    public void AddItem(Item item, int amount) => UpdateSlot(item, amount);

    // ���Կ��� �������� ����
    public void RemoveItem() => UpdateSlot(new Item(), 0);

    // ���Կ� �ִ� �������� ������ �ø��� ���
    public void AddAmount(int value) => UpdateSlot(item, amount += value);

    // �巡�� �� ��� �� ȣ��Ǵ� �Լ�
    public void UpdateSlot(Item item, int amount)
    {
        // ������ ���Կ� �ִ� �������� ó���ϴ� �׼�
        OnPreUpdate?.Invoke(this);

        this.item = item;
        this.amount = amount;

        // ���Ӱ� ������ �����ۿ� ���ؼ� ��� �ΰ����� �ൿ�� ����
        OnPostUpdate?.Invoke(this);
    }

    // �������� ���Կ� ������ �� �ִ��� �Ǵ��ϴ� �Լ�
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        // alloweditems �� �������� �ʾҰų�
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
