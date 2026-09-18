using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float switchTime = 0.7f;

    private int _index = 0;
    private float _timer = 0f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!(_timer > switchTime)) return;
        _timer = 0f;
        _index++;
        _index %= sprites.Length;
        _spriteRenderer.sprite = sprites[_index];
    }
}
