using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerSpawner : MonoBehaviour
{
    private GameObject pointer;

    public GameObject GetPointer(Vector3 pos)
    {
        pointer = ObjectPooler.Instance.GetObject("Pointer");
        pointer.transform.localScale = new Vector3(3f, 3f, 3f);
        pointer.transform.position = new Vector3(pos.x, pos.y + 0.18f, pos.z);
        pointer.SetActive(true);

        return pointer;
    }

    public void ReturnPointer(GameObject obj)
    {
        obj.SetActive(false);
    }
}
