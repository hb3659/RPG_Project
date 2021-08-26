using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Species species;

    public float radius;

    private float posX;
    private float posZ;
    public int maxSpawn;

    private Vector3 spawnPositioin;
    private Quaternion spawnRotation;

    private void Start()
    {
        StartCoroutine(UpdateSpawn());
    }

    IEnumerator UpdateSpawn()
    {
        while (true)
        {
            if (maxSpawn > transform.childCount)
            {
                posX = Random.Range(-radius, radius);
                posZ = Random.Range(-radius, radius);

                spawnPositioin = new Vector3(posX, this.transform.position.y, posZ);
                spawnRotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);

                GameObject enemy = ObjectPooler.Instance.GetObject(species.ToString());

                if (enemy != null)
                {
                    enemy.SetActive(true);
                    enemy.transform.SetParent(transform);
                    enemy.transform.localPosition = spawnPositioin;
                    enemy.transform.localRotation = spawnRotation;
                }
            }

            yield return new WaitForSeconds(7f);
        }
    }

    //private EnemySpawner instance;

    //public GameObject prefab;
    //[HideInInspector]
    //public Queue<GameObject> prefabQueue = new Queue<GameObject>();

    //public float radius;
    //public int maxSpawn;

    //private float posX;
    //private float posZ;

    //private Vector3 spawnPosition;
    //private Quaternion spawnRotation;

    //private void Awake()
    //{
    //    instance = this;
    //    Initialize(maxSpawn);

    //    StartCoroutine(UpdateSpawn());
    //}

    //private void Initialize(int maxSpawn)
    //{
    //    for (int i = 0; i < maxSpawn; i++)
    //        prefabQueue.Enqueue(CreateNewObject());
    //}

    //private GameObject CreateNewObject()
    //{
    //    GameObject go = Instantiate(prefab, this.transform);
    //    go.SetActive(false);
    //    go.transform.SetParent(this.transform);

    //    return go;
    //}

    //public GameObject GetObject()
    //{
    //    if (prefabQueue.Count > 0)
    //    {
    //        GameObject go = prefabQueue.Dequeue();
    //        go.SetActive(true);
    //        go.transform.SetParent(this.transform);

    //        return go;
    //    }
    //    else
    //    {
    //        GameObject newGo = CreateNewObject();
    //        newGo.SetActive(true);
    //        newGo.transform.SetParent(this.transform);

    //        return newGo;
    //    }
    //}

    //public void ReturnObject(GameObject go)
    //{
    //    go.SetActive(false);
    //    go.transform.SetParent(this.transform);

    //    prefabQueue.Enqueue(go);
    //}

    //IEnumerator UpdateSpawn()
    //{
    //    while (true)
    //    {
    //        if (prefabQueue.Count > 0)
    //        {
    //            posX = Random.Range(-radius, radius);
    //            posZ = Random.Range(-radius, radius);

    //            spawnPosition = new Vector3(posX, 0f, posZ);
    //            spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

    //            GameObject enemyObj = GetObject();
    //            enemyObj.transform.localPosition = spawnPosition;
    //            enemyObj.transform.localRotation = spawnRotation;
    //        }

    //        yield return new WaitForSeconds(7f);
    //    }
    //}
}
