using System.Collections;
using UnityEngine;

public class Hare : MonoBehaviour
{
    [SerializeField] private Sprite think, left, right;
    [SerializeField] private float thinkTime;
    [SerializeField] private AnimationCurve xScaleCurve, yScaleCurve, spriteCurve;
    [SerializeField] private float animationTime, animationMul = 0.5f;

    private SpriteRenderer _sr;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        NextThink();
    }

    public void NextThink()
    {
        StartCoroutine(ThinkRoutine());
    }

    private IEnumerator ThinkRoutine()
    {
        StartCoroutine(AnimationRoutine(think));
        
        float timer = 0f;
        while (timer < thinkTime) 
        {
            timer += Time.deltaTime;
            yield return null;
        }

        int dir = SortingLettersManager.Instance.CurrentDirection();
        StartCoroutine(AnimationRoutine(dir == 0 ? left : right));
    }

    private IEnumerator AnimationRoutine(Sprite after)
    {
        if (_sr.sprite == after) yield break;
        
        float timer = 0f;
        Sprite before = _sr.sprite;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            float ty = xScaleCurve.Evaluate(timer / animationTime);
            float tx = yScaleCurve.Evaluate(timer / animationTime);
            float ts = spriteCurve.Evaluate(timer / animationTime);

            transform.localScale = Vector3.one + new Vector3(tx, ty, 1f) * animationMul;
            _sr.sprite = ts > 0.5f ? after : before;
            
            yield return null;
        }
    }
}
