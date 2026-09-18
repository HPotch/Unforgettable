using TMPro;
using UnityEngine;

public class Guide : MonoBehaviour
{
    [SerializeField] private float showTime = 15f;
    
    private TextMeshPro _text;
    private float _timer;
    private bool _counting = false;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
        DialogueManager.Instance.onTextExit.AddListener(Quit);
        _text.color = new Color(1, 1, 1, 0);
    }

    public void StartCounting()
    {
        _counting = true;
    }

    private void Update()
    {
        if (!_counting) return;
        _timer += Time.deltaTime;
        _text.color = new Color(1, 1, 1, Mathf.Clamp01(_timer / showTime));
    }

    private void Quit()
    {
        Destroy(gameObject);
    }
}
