using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Letter : MonoBehaviour
{
    [SerializeField] private float animationTime = 0.7f;
    [SerializeField] private AnimationCurve positionCurve;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private Vector2 rotationDegrees = new Vector2(600, 900);

    public int correctStack;

    private void Awake()
    {
        correctStack = Random.Range(0, 2);
    }

    public void MoveToTarget(Vector2? target = null)
    {
        if (target == null) Debug.LogWarning("No target provided, moving to Vector2.zero");
        StartCoroutine(MoveRoutine(target));
    }

    private IEnumerator MoveRoutine(Vector2? target)
    {
        float timer = 0f;

        Vector2 startPos = transform.position;
        float startRotation =  transform.eulerAngles.z;
        float invert = (Random.Range(0, 2) * 2f) - 1f;
        print(invert);
        float targetRotation = startRotation + Random.Range(rotationDegrees.x, rotationDegrees.y) * invert;
        
        
        while (timer < animationTime)
        {
            float t = positionCurve.Evaluate(timer / animationTime);
            transform.position = Vector2.Lerp(startPos, target ?? Vector2.zero, t);
            t =  rotationCurve.Evaluate(timer / animationTime);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(startRotation, targetRotation, t));
            timer += Time.deltaTime;
            yield return null;
        }
        
        // Exit cleanly
        transform.position = target ?? Vector2.zero;
        transform.eulerAngles = new Vector3(0, 0, targetRotation);
    }
}
