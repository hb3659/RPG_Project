using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver;
    public static GameObject slotHoveredOver;
    public static GameObject tempItemBeingDragged;
}

// 마우스 입력이 중요
// 추상화를 통해 컴포넌트로 바로 사용하지 못하고
// 상속으로 사용할 수 있도록 설정
[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    // inventoryObject 에 대한 UI 랩핑
    public InventoryObject inventoryObject;
    private InventoryObject previousInventoryObject;

    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlotUIs();

        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            inventoryObject.Slots[i].parent = inventoryObject;
            // 아이템이 설정된 이후에 대한 액션 추가
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
        }

        // 마우스 포인터가 들어오거나 나가면 delegate 의 함수를 호출
        // 인벤토리 창에 있는 슬롯 아이템을 드래그 앤 드롭 했을 때
        // 창 내부(또는 외부)에서 일어났는지 확인하기 위함
        AddEvent(gameObject, EventTriggerType.PointerEnter,delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit,delegate { OnExitInterface(gameObject); });
    }

    protected virtual void Start()
    {
        // inventoryObject 의 슬롯들을 모두 갱신
        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }

    // 생성하는 슬롯들은 인벤토리 슬롯이 아닌 슬롯 ui
    public abstract void CreateSlotUIs();

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();

        if (!trigger)
        {
            Debug.LogWarning("No EventTrigger component found!");
            return;
        }

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite =
            slot.item.id < 0 ? null : slot.itemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().color =
            slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text =
            slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString());
    }

    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnEnterSlot(GameObject go)
    {
        MouseData.slotHoveredOver = go;
    }

    public void OnExitSlot(GameObject go)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnStartDrag(GameObject go)
    {
        MouseData.tempItemBeingDragged = CreateDragImage(go);
    }

    // 드래그 시 마우스를 따라오는 임시 이미지 생성
    private GameObject CreateDragImage(GameObject go)
    {
        if (slotUIs[go].item.id < 0)
            return null;

        GameObject dragImageGo = new GameObject();

        RectTransform rectTransform = dragImageGo.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        dragImageGo.transform.SetParent(transform.parent);

        Image image = dragImageGo.AddComponent<Image>();
        image.sprite = slotUIs[go].itemObject.icon;
        image.raycastTarget = false;

        dragImageGo.name = "Drag Image";

        return dragImageGo;
    }

    public void OnDrag(GameObject go)
    {
        if (MouseData.tempItemBeingDragged == null)
            return;

        MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position =
            Input.mousePosition;
    }

    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.tempItemBeingDragged);

        // 창 외부에서 마우스를 놓게 되면 아이템을 버린다.
        if (MouseData.interfaceMouseIsOver == null)
            slotUIs[go].RemoveItem();
        else if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotData);
        }
    }
}
