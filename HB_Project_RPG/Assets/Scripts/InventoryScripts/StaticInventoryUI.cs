using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventoryUI : InventoryUI
{
    // ������ �����ϴ� ���� �ƴ� �̸� ������ ������ ���
    public GameObject[] staticSlots = null;

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for(int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            // ���� �̹� ���Ե��� �����Ǿ� �ֱ� ������ Instantiate ���� �ʴ´�.
            GameObject uiGo = staticSlots[i];

            AddEvent(uiGo, EventTriggerType.PointerEnter, delegate { OnEnterSlot(uiGo); });
            AddEvent(uiGo, EventTriggerType.PointerExit, delegate { OnExitSlot(uiGo); });
            AddEvent(uiGo, EventTriggerType.BeginDrag, delegate { OnStartDrag(uiGo); });
            AddEvent(uiGo, EventTriggerType.EndDrag, delegate { OnEndDrag(uiGo); });
            AddEvent(uiGo, EventTriggerType.Drag, delegate { OnDrag(uiGo); });

            inventoryObject.Slots[i].slotUI = uiGo;
            slotUIs.Add(uiGo, inventoryObject.Slots[i]);

            // Debug ��
            uiGo.name += ": " + i;
        }
    }
}
