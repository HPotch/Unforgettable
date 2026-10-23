using System.Collections;
using UnityEngine;

public class ParallexManager : MonoBehaviour
{
    public static ParallexManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Vector2 position = Vector2.zero;

    public void SetPosition(Vector2 target, float time = 1f)
    {
        StartCoroutine(PositionRoutine(target, time));
    }

    private IEnumerator PositionRoutine(Vector2 target, float time)
    {
        float timer = 0f;
        Vector2 startPos = position;

        while (timer < time)
        {
            timer += Time.deltaTime;
            float t =  timer / time;
            position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }
        
        position = target;
    }
}
