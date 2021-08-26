using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextObject
{
    HeroText,
    EnemyText,
}

public class CameraFacing_FloatingDamage : CameraFacing
{
    public TextObject textObject;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public void updateDamage(Text damageText, float damage)
    {
        GameObject text = ObjectPooler.Instance.GetObject(textObject.ToString());

        text.SetActive(true);
        text.transform.SetParent(transform);

        damageText = text.GetComponent<Text>();
        damageText.text = damage.ToString();

        //Debug.Log("position : " + damageText.rectTransform.position);
        //Debug.Log("rotation : " + damageText.rectTransform.rotation);
        // position 값 변화생긴다.

        damageText.rectTransform.localPosition = new Vector3(0f, transform.position.y, 0f);
        damageText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        StartCoroutine(ReturnObject(text));
    }
    IEnumerator ReturnObject(GameObject text)
    {
        yield return new WaitForSeconds(1f);

        ObjectPooler.Instance.ReturnObject(text);
    }
}
