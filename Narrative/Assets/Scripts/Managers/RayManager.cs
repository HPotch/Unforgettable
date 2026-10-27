using UnityEngine;
using UnityEngine.InputSystem;

public class RayManager : MonoBehaviour
{
    // Manager
    public static RayManager Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    // Public variable
    [Header("Output")]
    public Collider2D mouseOver;
    public Vector2 mouseWorldPos;
    
    // Settings
    [Header("Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask preferMask;

    private void Start()
    {
        if (!cam) cam = Camera.main;
    }

    private void Update()
    {
        // Get mouseOver
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray gameRay = cam.ScreenPointToRay(mouseScreenPos);
        RaycastHit2D preferHit = Physics2D.GetRayIntersection(gameRay, Mathf.Infinity, preferMask);
        
        if (preferHit.collider)
            mouseOver = preferHit.collider;
        else
        {
            RaycastHit2D gameHit = Physics2D.GetRayIntersection(gameRay);
            mouseOver = gameHit.collider;
        }
        
        mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
    }
}
