using System.Collections;
using UnityEngine;

public class Hare : MonoBehaviour
{
    [SerializeField] private Sprite think, left, right;
    [SerializeField] private float thinkTime;
    [SerializeField] private AnimationCurve xScaleCurve, yScaleCurve, spriteCurve;
    [SerializeField] private float animationTime;

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
        _sr.sprite = think;
        
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
        print("change");
        float timer = 0f;
        Sprite before = _sr.sprite;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            float ty = xScaleCurve.Evaluate(timer / animationTime);
            float tx = yScaleCurve.Evaluate(timer / animationTime);
            float ts = spriteCurve.Evaluate(timer / animationTime);

            transform.localScale = new Vector3(tx, ty, 1f);
            _sr.sprite = ts > 0.5f ? after : before;
            
            yield return null;
        }
    }
}
