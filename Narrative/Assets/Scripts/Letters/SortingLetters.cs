using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SortingLetters : MonoBehaviour
{
    [SerializeField] private float appearWait = 1f, startWait = 1f;
    [SerializeField] private UnityEvent onAppear, onStart;
    
    public void StartGame()
    {
        StartCoroutine(WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(appearWait);
        onAppear.Invoke();
        yield return new WaitForSeconds(startWait);
        onStart.Invoke();
    }
}
