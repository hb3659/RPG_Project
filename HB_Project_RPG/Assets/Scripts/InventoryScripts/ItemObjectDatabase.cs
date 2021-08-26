using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item Database", menuName ="Inventory System/Items/Database")]
public class ItemObjectDatabase : ScriptableObject
{
    public ItemType type;
    public ItemObject[] itemObjects;

    private int baseID;

    public int setBaseID()
    {
        switch (type)
        {
            case ItemType.Helmet:
                baseID = 101;
                return baseID;

            case ItemType.Chest:
                baseID = 201;
                return baseID;

            case ItemType.Belt:
                baseID = 301;
                return baseID;

            case ItemType.Boots:
                baseID = 401;
                return baseID;

            case ItemType.Pauldrons:
                baseID = 501;
                return baseID;

            case ItemType.Gloves:
                baseID = 601;
                return baseID;

            case ItemType.LeftWeapon:
                baseID = 701;
                return baseID;

            case ItemType.RightWeapon:
                baseID = 801;
                return baseID;

            case ItemType.Food:
                baseID = 901;
                return baseID;

            default:
                baseID = 1001;
                return baseID;
        }
    }

    public void OnValidate()
    {
        setBaseID();

        if (itemObjects.Length > 0)
        {
            for (int i = 0; i < itemObjects.Length; ++i)
            {
                itemObjects[i].data.id = baseID + i;
            }
        }
    }
}
