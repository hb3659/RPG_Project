using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType : int
{
    Helmet = 0,
    Chest = 1,
    Belt = 2,
    Boots = 3,
    Pauldrons = 4,
    Gloves = 5,
    LeftWeapon = 6,
    RightWeapon = 7,
    Food = 8,
    Default = 9,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public bool stackable;

    public Sprite icon;
    public GameObject modelPrefab;

    public Item data = new Item();

    public List<string> boneNames = new List<string>();

    [TextArea(15, 20)]
    public string description;

    // 인스펙터에서 값이 변경될 경우 호출되는 함수
    // boneName 을 위함
    private void OnValidate()
    {
        boneNames.Clear();

        if (modelPrefab == null ||
            modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
            return;

        SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;

        foreach(Transform t in bones)
        {
            boneNames.Add(t.name);
        }
    }

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}
