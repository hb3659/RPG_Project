using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject player;

    void Start()
    {
        player = GetComponent<GameObject>();
        player =
            Instantiate(playerPrefabs[(int)DataManager.instance.currentCharacter]);
        player.transform.position = transform.position;

        player.name = playerPrefabs[(int)DataManager.instance.currentCharacter].name;
    }
}
