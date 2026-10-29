using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hare : MonoBehaviour
{
    public bool lying = false;
    
    [SerializeField] private Sprite think, left, right;
    [SerializeField] private float thinkTime;
    [SerializeField] private AnimationCurve xScaleCurve, yScaleCurve, spriteCurve;
    [SerializeField] private float animationTime, animationMul = 0.5f;
    [SerializeField] private Vector2Int randomLieIndex = new Vector2Int(5, 10);

    private SpriteRenderer _sr;
    private int _index, _lieIndex;
    private Coroutine _thinkRoutine, _animationRoutine;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _lieIndex = Random.Range(randomLieIndex.x, randomLieIndex.y + 1);
    }

    private void Start()
    {
        NextThink();
    }
    
    public void NextThink()
    {
        _index++;
        if (_index >= _lieIndex) lying = true;
        
        if (_thinkRoutine != null) StopCoroutine(_thinkRoutine); // Foremost animation passes
        _thinkRoutine = StartCoroutine(ThinkRoutine());
    }

    private IEnumerator ThinkRoutine()
    {
        if (_animationRoutine !=  null) StopCoroutine(_animationRoutine);
        _animationRoutine = StartCoroutine(AnimationRoutine(think));
        
        float timer = 0f;
        while (timer < thinkTime) 
        {
            timer += Time.deltaTime;
            yield return null;
        }

        bool rightDir = SortingLettersManager.Instance.CurrentDirection() == 0; // Get the right direction
        bool dir = lying ? !rightDir : rightDir; // Flip it if lying is enabled
        
        StartCoroutine(AnimationRoutine(dir ? left : right));
        
        _thinkRoutine = null;
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

        _animationRoutine = null;
    }

    private void Update()
    {
        if (!SortingLettersManager.Instance.end) return;
        if (_thinkRoutine != null) StopCoroutine(_thinkRoutine);
        if (_animationRoutine != null) StopCoroutine(_animationRoutine);
    }
}
