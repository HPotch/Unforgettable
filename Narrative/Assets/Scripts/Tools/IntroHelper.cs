using UnityEngine;
using DialogueComponent = DialogueManager.DialogueComponent;

[ExecuteInEditMode]
public class DialogueHelper : MonoBehaviour
{
    [SerializeField] private bool resetAlpha;
    [SerializeField] private Color setColor;
    
    private DialogueCaller _dialogueCaller;

    private Color _prevColor;
    
    private void Awake()
    {
        _prevColor = setColor;
        _dialogueCaller = GetComponent<DialogueCaller>();
    }

    private void Update()
    {
        ResetAlpha();
        SetColor();
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
}
