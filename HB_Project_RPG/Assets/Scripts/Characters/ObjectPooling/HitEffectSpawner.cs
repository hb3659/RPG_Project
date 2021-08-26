using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Effect
{
    Hit,
    WarriorAttack,
    EnemyAttack,
}

public class HitEffectSpawner : MonoBehaviour
{
    public Effect effect;
    private GameObject hit;
    private GameObject attack;

    public GameObject GetHit()
    {
        hit = ObjectPooler.Instance.GetObject(effect.ToString());
        hit.transform.position = transform.position;
        hit.SetActive(true);
        hit.transform.SetParent(transform);

        StartCoroutine(ReturnEffect(hit));

        return hit;
    }

    public GameObject Attack()
    {
        attack = ObjectPooler.Instance.GetObject(effect.ToString());
        attack.transform.position = transform.position;
        attack.SetActive(true);
        attack.transform.SetParent(transform);

        StartCoroutine(ReturnEffect(attack));

        return attack;
    }

    IEnumerator ReturnEffect(GameObject obj)
    {
        yield return new WaitForSeconds(2f);

        ObjectPooler.Instance.ReturnObject(obj);
    }
}
