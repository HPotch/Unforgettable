using UnityEngine;
using UnityEngine.Events;

public class SmoothMove : MonoBehaviour
{
    [SerializeField] private float moveSmoothness = 20f;
    public UnityEvent beforeMove;
    public UnityEvent afterMove;
    
    private Vector2 prevPos = Vector2.zero;

    private void Start()
    {
        prevPos = transform.position;
    }

    private void Update()
    {
        beforeMove.Invoke();
        transform.position = Vector2.Lerp(prevPos, transform.position, moveSmoothness * Time.deltaTime);
        afterMove.Invoke();
        prevPos = transform.position;
    }
}
