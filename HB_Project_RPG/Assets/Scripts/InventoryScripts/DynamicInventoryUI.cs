using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject slotPrefab;

    public Transform content;

    // 슬롯의 시작 위치
    [SerializeField]
    protected Vector2 start;
    // 슬롯의 크기
    [SerializeField]
    protected Vector2 size;
    // 슬롯 사이의 간격
    [SerializeField]
    protected Vector2 space;
    // 한 행에 몇 개의 슬롯을 설정할 것인지
    [Min(1), SerializeField]
    protected int numberOfColum = 4;

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            GameObject uiGo;

            if (content)
                uiGo = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, content);
            else
                uiGo = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);

            uiGo.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

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

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColum));
        float y = start.y + (-(space.y + size.y) * (i / numberOfColum));

        return new Vector3(x, y, 0f);
    }
}
