using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitEffectSpawner : MonoBehaviour
{
    private static Enemy_HitEffectSpawner instance;

    public int maxIndex = 3;

    public GameObject prefab;
    [HideInInspector]
    public Queue<GameObject> prefabQueue = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
        Initialize(maxIndex);
    }

    private void Initialize(int maxIndex)
    {
        for (int i = 0; i < maxIndex; i++)
            prefabQueue.Enqueue(CreateNewObject());
    }

    private GameObject CreateNewObject()
    {
        GameObject go = Instantiate(prefab, this.transform);
        go.SetActive(false);
        go.transform.SetParent(this.transform);

        return go;
    }

    public GameObject GetObject()
    {
        if (instance.prefabQueue.Count > 0)
        {
            GameObject go = prefabQueue.Dequeue();
            go.SetActive(true);
            go.transform.SetParent(this.transform);

            return go;
        }
        else
        {
            GameObject newGo = CreateNewObject();
            newGo.SetActive(true);
            newGo.transform.SetParent(this.transform);

            return newGo;
        }
    }

    public void ReturnObject(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(this.transform);

        instance.prefabQueue.Enqueue(go);
    }
}
