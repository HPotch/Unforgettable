using UnityEngine;

public class Toothbrush : MonoBehaviour
{
    [SerializeField] private Trigger trigger;
    [SerializeField] private Collider2D toothPaste;
    [SerializeField] private Sprite withToothPaste;
    [SerializeField] private Polaroid polaroid;
    private SpriteRenderer _sr;
    
    private void Awake()
    {
        trigger.onTriggerEnter.AddListener(Trigger);
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Trigger()
    {
        if (trigger.other != toothPaste) return;
        _sr.sprite = withToothPaste;
        polaroid.ShowPolaroid();
    }
}
