using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float switchTime = 0.7f;
    [SerializeField] private bool oneShot = false;
    [SerializeField] private bool preview;

    private int _index = 0;
    private float _timer = 0f;
    private SpriteRenderer _spriteRenderer;
    private Image _image;

    private bool _useImage = false;
    private bool  _playing = true;

    private void Awake()
    {
        if (oneShot) _playing = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer) return;
        _image =  GetComponent<Image>();
        _useImage = true;
        if (!_image) Debug.LogError("No SpriteRenderer or Image component found");
    }

    public void StartAnimation()
    {
        _playing = true;
    }

    private void Update()
    {
        if (!_playing && !preview) return;
        _timer += Time.deltaTime;

        if (!(_timer > switchTime)) return;
        _timer = 0f;
        _index++;

        // Exit if last frame and oneShot
        if (_index >= sprites.Length && oneShot)
        {
            _playing = false;
            return;
        }
        
        _index %= sprites.Length;
        
        if (_useImage) _image.sprite = sprites[_index];
        else _spriteRenderer.sprite = sprites[_index];
        
    }
}
