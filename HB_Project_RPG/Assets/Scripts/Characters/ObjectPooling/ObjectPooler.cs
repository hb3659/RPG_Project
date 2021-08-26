using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ObjectPoolItems
{
    public int amountToPool;
    // ������ ������
    public GameObject objectToPool;
    public bool shouldExpand;
}

public class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler instance = null;

    public static ObjectPooler Instance
    {
        get { return instance; }
    }

    public List<ObjectPoolItems> itemsToPool = new List<ObjectPoolItems>();
    public List<GameObject> PooledObjects;

    private void Awake()
    {
        // �ٸ� ������ �������� ���ϵ���
        if (instance == null)
            instance = this;
        else
            Destroy(Instance);

        Initialize();
    }

    private void Initialize()
    {
        PooledObjects = new List<GameObject>();

        // ������ ���� ������ ����(��Ȱ��ȭ)
        foreach (ObjectPoolItems item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = CreateNewObject(item);
            }
        }
    }

    private GameObject CreateNewObject(ObjectPoolItems item)
    {
        GameObject obj = Instantiate(item.objectToPool);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        PooledObjects.Add(obj);

        return obj;
    }

    public GameObject GetObject(string tag)
    {
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy && PooledObjects[i].tag == tag)
                return PooledObjects[i];
        }

        foreach (ObjectPoolItems item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = CreateNewObject(item);

                    return obj;
                }
            }
        }

        return null;
    }

    public void ReturnObject(GameObject pooled)
    {
        pooled.SetActive(false);
        pooled.transform.SetParent(transform);
    }
}
