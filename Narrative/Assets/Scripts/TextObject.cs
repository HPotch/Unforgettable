using UnityEngine;
using UnityEngine.Events;

public class TextObject : MonoBehaviour
{
    public UnityEvent OnEntered;
    public UnityEvent OnExited;
    
    public void Entered()
    {
        OnEntered.Invoke();
    }

    public void Exited()
    {
        OnExited.Invoke();
    }
}
