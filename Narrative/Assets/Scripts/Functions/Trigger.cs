using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public Collider2D other;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        other = collision;
        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit.Invoke();
    }
}
