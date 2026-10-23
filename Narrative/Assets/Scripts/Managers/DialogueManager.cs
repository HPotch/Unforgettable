using System;
using TMPro;
using UnityEngine;
using EasyTextEffects;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
    // Manager
    public static DialogueManager Instance;

    [Header("References")]
    public Transform textObject;
    
    // Public variables
    [NonSerialized] public bool Completed = true;
    [NonSerialized] public bool Exiting = false;
    [NonSerialized] public float TimeSinceText = 0f;
    [NonSerialized] public DialogueComponent TargetDC;
    
    // Events
    public UnityEvent onTextExit;
    public UnityEvent onTextEnter;
    
    // Private references
    private TextMeshProUGUI _textMesh;
    private TextEffect _textEffect;
    private TextObject _textObject;
    private Material _textMaterial;
    
    // Enums
    public enum SkipState
    {
        Skippable,
        OnlyTimeSkip,
        NoSkip
    }
    
    // Custom classes
    [Serializable]
    public class DialogueComponent
    {
        public string text;
        public Color color = Color.white;
        public float fontSize = 6f;
        public float autoSkipTime = 0f;
        public SkipState skipState = SkipState.Skippable;
        public UnityEvent OnStart;
        public UnityEvent OnEnd;
    }
    
    private void Awake()
    {
        // Manager
        if (!Instance) Instance = this;
        else Destroy(gameObject);
        
        SetTextVariables();
    }

    public void SetTextVariables()
    {
        // Remove previous listeners
        if (_textObject)
        {
            _textObject.OnEntered.RemoveListener(Complete);
            _textObject.OnExited.RemoveListener(NextDialogue);
        }
        
        // Make variables
        _textMesh = textObject.GetComponent<TextMeshProUGUI>();
        _textEffect = textObject.GetComponent<TextEffect>();
        _textObject = textObject.GetComponent<TextObject>();
        _textMaterial = _textMesh.fontMaterial;
        _textObject.OnEntered.AddListener(Complete);
        _textObject.OnExited.AddListener(NextDialogue);
    }

    private void Update()
    {
        TimeSinceText += Time.deltaTime;
    }

    public void Complete()
    {
        _textEffect.StopAllEffects();
        _textMesh.color = Color.white;
        _textMaterial.SetColor("_FaceColor", TargetDC.color);
        Completed = true;
    }
    
    public void ChangeDialogue(DialogueComponent text)
    {
        if (Exiting) return;
        TargetDC?.OnEnd.Invoke();
        if (TargetDC != null && TargetDC?.text != "") onTextExit.Invoke();
        TargetDC = text;
        if (_textMesh.text == "")
        {
            NextDialogue();
            return;
        }
        _textEffect.StartManualEffect("ColorOut");
        _textEffect.StartManualEffect("ScaleOut");
        _textEffect.StartManualEffect("MoveOut");
        Exiting = true;
    }

    private void NextDialogue()
    {
        _textMesh.text = TargetDC.text;
        _textMesh.fontSize = TargetDC.fontSize;
        _textMesh.color = new Color(1, 1, 1, 0);
        _textMaterial.SetColor("_FaceColor", TargetDC.color);
        _textEffect.Refresh();
        TargetDC.OnStart.Invoke();
        if (TargetDC?.text != "")onTextEnter.Invoke();
        TimeSinceText = 0f;
        Completed = false;
        Exiting = false;
    }

    
}
