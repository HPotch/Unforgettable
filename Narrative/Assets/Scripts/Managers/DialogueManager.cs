using System;
using TMPro;
using UnityEngine;
using EasyTextEffects;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    // Manager
    public static DialogueManager Instance;

    [Header("References")]
    [SerializeField] private Transform textObject;
    
    // Public variables
    [NonSerialized] public bool Completed = true;
    [NonSerialized] public bool Exiting = false;
    [NonSerialized] public float TimeSinceText = 0f;
    [NonSerialized] public DialogueComponent TargetDC;
    
    // Private references
    private TextMeshPro _textMesh;
    private TextEffect _textEffect;
    private TextObject _textObject;
    private Material _textMaterial;
    
    // Custom classes
    [Serializable]
    public class DialogueComponent
    {
        public String text;
        public Color color = Color.white;
        public float fontSize = 6f;
        public float autoSkipTime = 0f;
        public UnityEvent OnStart;
        public UnityEvent OnEnd;
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
        TargetDC = text;
        if (_textMesh.text == "")
        {
            NextDialogue();
            return;
        }
        _textEffect.StartManualEffect("ColorOut");
        _textEffect.StartManualEffect("ScaleOut");
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
        TimeSinceText = 0f;
        Completed = false;
        Exiting = false;
    }

    
}
