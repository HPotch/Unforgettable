using System;
using UnityEngine;

public class Parallex : MonoBehaviour
{
    [Serializable] private enum Smoothmode
    {
        None,
        Smoothed,
        Velocity
    }

    [Serializable]
    public class Axis
    {
        public bool x, y, z;
    }
    
    [Header("Settings")]
    [SerializeField] private float depth = 10f;
    [SerializeField] private float smooth = 50f;
    [SerializeField] private float rotateAmount = 0f;
    [SerializeField] private Smoothmode smoothMode = Smoothmode.None;
    [SerializeField] private Axis ignorePos, ignoreRot;

    [NonSerialized] public Vector2 StartPos;

    private Vector2 velocity, smoothedMousePos, pos;
    private Vector3 _backUpPos, _backUpRot;

    private void Awake()
    {
        StartPos = transform.position;
        pos = transform.position;
        _backUpPos = transform.localPosition;
        _backUpRot = transform.localEulerAngles;
    }

    private void Update()
    {
        Vector2 basePos = StartPos;
        
        // Smooth input when using velocity
        if (smoothMode == Smoothmode.Velocity)
        {
            velocity = Vector2.Lerp(velocity, RayManager.Instance.mouseWorldPos - smoothedMousePos, smooth * Time.deltaTime);
            smoothedMousePos += velocity;
        }
        else
            smoothedMousePos = RayManager.Instance.mouseWorldPos;
        
        // Set targets
        Vector2 posTarget = basePos + smoothedMousePos / depth;
        Vector2 rotTarget = new Vector2(smoothedMousePos.y - pos.y, smoothedMousePos.x - pos.x) * rotateAmount;

        // Apply transforms
        if (smoothMode == Smoothmode.Smoothed)
        {
            pos = Vector2.Lerp(pos, posTarget, Time.deltaTime * smooth);
            transform.eulerAngles = new Vector3(
                Mathf.LerpAngle(transform.eulerAngles.x, rotTarget.x, Time.deltaTime * smooth),
                Mathf.LerpAngle(transform.eulerAngles.y, rotTarget.y, Time.deltaTime * smooth),
                0f);
        }
        else
        {
            pos = posTarget;
            transform.eulerAngles = rotTarget;
        }
        
        Vector2 camPos = (ParallexManager.Instance?.position ?? Vector3.zero) / depth;
        transform.position = pos + camPos;

        Ignore();
    }

    private void Ignore()
    {
        transform.localPosition = new Vector3(
            ignorePos.x ? _backUpPos.x : transform.localPosition.x,
            ignorePos.y ? _backUpPos.y : transform.localPosition.y,
            ignorePos.z ? _backUpPos.z : transform.localPosition.z);
        transform.localEulerAngles = new Vector3(
            ignoreRot.x ? _backUpRot.x : transform.localEulerAngles.x,
            ignoreRot.y ? _backUpRot.y : transform.localEulerAngles.y,
            ignoreRot.z ? _backUpRot.z : transform.localEulerAngles.z);
    }
}
