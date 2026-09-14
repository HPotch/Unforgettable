using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 targetSize = new Vector2(1.05f, 1.05f);
    [SerializeField] private float scaleTime = 0.5f;
    [SerializeField] private AnimationCurve scaleCurve;

    private Vector2 _startSize = Vector2.one;
    private float _timer = 0f;
    private RayManager _rayManager;
    private Movable _movable;
    private void Start()
    {
        _rayManager = RayManager.Instance;
        _startSize = transform.localScale;
        _movable = GetComponent<Movable>();
    }

    private void Update()
    {
        // Set hovered
        bool hovered = false;
        if (_rayManager.mouseOver)
            hovered = _rayManager.mouseOver.transform == transform || _rayManager.mouseOver.transform.parent == transform;
        if (_movable)
            if (_movable.PickedUp)
                hovered = false;
        // Set timer
        _timer += Time.deltaTime * (hovered ? 1f : -1f);
        _timer = Mathf.Clamp(_timer, 0f, scaleTime);
        // Set scale
        float t = scaleCurve.Evaluate(_timer / scaleTime);
        transform.localScale = Vector3.LerpUnclamped(_startSize, targetSize, t);
    }
}
