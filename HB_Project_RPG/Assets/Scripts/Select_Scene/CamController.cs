using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{
    #region Variables
    public Camera cam;
    public Canvas canvas;
    #endregion Variables

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        canvas.gameObject.SetActive(false);
    }

    private IEnumerator IEMoveToSopt(GameObject target)
    {
        float startTime = Time.time;
        float duration = 1.2f;
        Vector3 startPos = cam.transform.localPosition;
        Vector3 endPos = target.transform.position;

        while (Time.time - startTime <= duration)
        {
            cam.transform.localPosition =
                Vector3.Lerp(startPos, endPos, (Time.time - startTime) / duration);

            yield return null;
        }
        transform.localPosition = endPos;
        canvas.gameObject.SetActive(true);
    }

    public void MoveToSpot(GameObject target)
    {
        StartCoroutine(IEMoveToSopt(target));
    }
    #endregion Function
}
