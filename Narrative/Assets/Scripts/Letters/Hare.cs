using System.Collections;
using UnityEngine;

public class Hare : MonoBehaviour
{
    [SerializeField] private Sprite think, left, right;
    [SerializeField] private float thinkTime;

    private SpriteRenderer _sr;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
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
        _sr.sprite = dir == 0 ? left : right;
    }
}
