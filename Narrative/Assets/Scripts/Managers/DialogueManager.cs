using System;
using TMPro;
using UnityEngine;
using EasyTextEffects;

public class DialogueManager : MonoBehaviour
{
    // Manager
    public static DialogueManager Instance;

    [Header("References")]
    [SerializeField] private Transform textObject;
    
    // Public variables
    [NonSerialized] public bool completed = true;
    [NonSerialized] public bool exiting = false;
    
    // Private references
    private TextMeshPro _textMesh;
    private TextEffect _textEffect;
    private TextObject _textObject;
    private Material _textMaterial;
    
    //  Private variables
    private String _targetText = "";
    private Color _targetColor = Color.white;
    private float _targetFontSize = 6f;
    
    // Custom classes
    [Serializable]
    public class DialogueComponent
    {
        public String text;
        public Color color = Color.white;
        public float fontSize = 6f;
    }
    
    private void Awake()
    {
        // Manager
        if (!Instance) Instance = this;
        else Destroy(gameObject);
        
        // Reference setup
        _textMesh = textObject.GetComponent<TextMeshPro>();
        _textEffect = textObject.GetComponent<TextEffect>();
        _textObject = textObject.GetComponent<TextObject>();
        _textMaterial = _textMesh.fontMaterial;
        _textObject.OnEntered.AddListener(Complete);
        _textObject.OnExited.AddListener(NextDialogue);
    }
    
    public void Complete()
    {
        _textEffect.StopAllEffects();
        _textMesh.color = Color.white;
        _textMaterial.SetColor("_FaceColor", _targetColor);
        completed = true;
    }
    
    public void ChangeDialogue(DialogueComponent text)
    {
        if (exiting) return;
        _targetText = text.text;
        _targetColor = text.color;
        _targetFontSize = text.fontSize;
        if (_textMesh.text == "")
        {
            NextDialogue();
            return;
        }
        _textEffect.StartManualEffect("ColorOut");
        _textEffect.StartManualEffect("ScaleOut");
        exiting = true;
    }

    private void NextDialogue()
    {
        _textMesh.text = _targetText;
        _textMesh.fontSize = _targetFontSize;
        _textMesh.color = new Color(1, 1, 1, 0);
        _textMaterial.SetColor("_FaceColor", _targetColor);
        _textEffect.Refresh();
        completed = false;
        exiting = false;
    }

    
}
