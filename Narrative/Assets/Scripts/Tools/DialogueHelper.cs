using UnityEngine;
using DialogueComponent = DialogueManager.DialogueComponent;

[ExecuteInEditMode]
public class DialogueHelper : MonoBehaviour
{
    [SerializeField] private bool resetAlpha;
    [SerializeField] private Color setColor;
    [SerializeField] private float setFontSizes;
    
    private DialogueCaller _dialogueCaller;

    private Color _prevColor;
    private float _prevFontSize;
    
    private void Awake()
    {
        _prevColor = setColor;
        _prevFontSize = setFontSizes;
        _dialogueCaller = GetComponent<DialogueCaller>();
    }

    private void Update()
    {
        ResetAlpha();
        SetColor();
        SetFontSizes();
    }

    private void ResetAlpha()
    {
        if (!resetAlpha) return;
        foreach (DialogueComponent dc in _dialogueCaller.dialogues)
            dc.color.a = 1f;
    }

    private void SetColor()
    {
        if (setColor == _prevColor) return;
        foreach (DialogueComponent dc in _dialogueCaller.dialogues)
            dc.color = setColor;
        _prevColor = setColor;
    }

    private void SetFontSizes()
    {
        if (setFontSizes == _prevFontSize) return;
        foreach (DialogueComponent dc in _dialogueCaller.dialogues)
            dc.fontSize = setFontSizes;
        _prevFontSize = setFontSizes;
    }
}
