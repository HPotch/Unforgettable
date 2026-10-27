using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SortingLetters : MonoBehaviour
{
    [SerializeField] private float appearWait = 2f;
    [SerializeField] private float startWait = 3f;
    [SerializeField] private UnityEvent onAppear;
    [SerializeField] private UnityEvent onStart;
    
    private void Start()
    {
        StartCoroutine(WaitRoutine(appearWait, onAppear));
        StartCoroutine(WaitRoutine(startWait, onStart));
    }

    private IEnumerator WaitRoutine(float time, UnityEvent action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}
