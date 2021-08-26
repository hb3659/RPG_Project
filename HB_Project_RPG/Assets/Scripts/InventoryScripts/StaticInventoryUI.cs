using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventoryUI : InventoryUI
{
    // 슬롯을 생성하는 것이 아닌 미리 생성된 슬롯을 사용
    public GameObject[] staticSlots = null;

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for(int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            // 씬에 이미 슬롯들이 생성되어 있기 때문에 Instantiate 하지 않는다.
            GameObject uiGo = staticSlots[i];

            AddEvent(uiGo, EventTriggerType.PointerEnter, delegate { OnEnterSlot(uiGo); });
            AddEvent(uiGo, EventTriggerType.PointerExit, delegate { OnExitSlot(uiGo); });
            AddEvent(uiGo, EventTriggerType.BeginDrag, delegate { OnStartDrag(uiGo); });
            AddEvent(uiGo, EventTriggerType.EndDrag, delegate { OnEndDrag(uiGo); });
            AddEvent(uiGo, EventTriggerType.Drag, delegate { OnDrag(uiGo); });

            inventoryObject.Slots[i].slotUI = uiGo;
            slotUIs.Add(uiGo, inventoryObject.Slots[i]);

            // Debug 용
            uiGo.name += ": " + i;
        }
    }
}
