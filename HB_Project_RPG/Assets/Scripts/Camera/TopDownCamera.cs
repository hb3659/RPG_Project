using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    #region Vaiables
    public float height = 5f;
    public float distance = 10f;
    public float angle = 45f;
    public float lookAtHeight = 2f;
    public float smoothSpeed = 0.5f;
    private float zoomSpeed = 10.0f;

    private Vector3 refVelocity;
    private Vector3 worldDefaultForward;

    public Transform target;

    private Camera cam;
    #endregion Variables

    private void Start()
    {
        cam = GetComponent<Camera>();
        worldDefaultForward = transform.forward;
    }
    private void LateUpdate()
    {
        HandleCamera();
        ZoomInOut();
    }

    public void HandleCamera()
    {
        if (!target)
            return;

        // Build world position vector
        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);

        // Build our rotated vector
        Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;

        // Move our position
        Vector3 finalTargetPosition = target.position;
        finalTargetPosition.y += lookAtHeight;

        Vector3 finalPosition = finalTargetPosition + rotatedVector;

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);

        transform.LookAt(target.position);
    }

    public void ZoomInOut()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;

            if (cam.fieldOfView <= 20f && scroll < 0)
                cam.fieldOfView = 20f;
            else if (cam.fieldOfView >= 60f && scroll > 0)
                cam.fieldOfView = 60f;
            else
                cam.fieldOfView += scroll;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        if (target)
        {
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);
        }

        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
