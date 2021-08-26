using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CameraFacing : MonoBehaviour
{
    [HideInInspector]
    public Camera referenceCamera;
    public bool reverseFace = false;

    public enum Axis
    {
        up,
        down,
        left,
        right,
        forward,
        back,
    };

    public Axis axis = Axis.up;

    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;

            case Axis.forward:
                return Vector3.forward;

            case Axis.back:
                return Vector3.back;

            case Axis.left:
                return Vector3.left;

            case Axis.right:
                return Vector3.right;
        }

        return Vector3.up;
    }
    protected virtual void Awake()
    {
        if (!referenceCamera)
            referenceCamera = Camera.main;
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
        Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);

        transform.LookAt(targetPos, targetOrientation);
    }
}
