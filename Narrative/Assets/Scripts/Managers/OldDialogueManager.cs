// This is an old dialogue manager, it is broken
// I was meant to add a new textMesh for each letter, so that I could smoothly let each letter enter the scene
// This however, introduced a bunch of bugs, and it is quite heavy to load
// I have turned to a shader-based approach

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OldDialogueManager : MonoBehaviour
{
    // Manager
    public static OldDialogueManager Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    [Header("Settings")]
    [SerializeField] private float characterTime = 0.1f;
    [SerializeField] private float fadeOutTime = 0.5f;
    [SerializeField] private Color defaultTextColor = Color.white;
    [SerializeField] private float letterScaleEffect = 0.2f;
    [SerializeField] private float letterMoveEffect = 0.1f;
    [SerializeField] private AnimationCurve fadeInCurve;
    [SerializeField] private AnimationCurve fadeOutCurve;

    [Header("References")]
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private Transform textParent;
    

    // Custom classes
    public class Letter
    {
        public Transform  Transform;
        public TextMeshPro Text;
        public Vector2 StartPosition;
    }
    
    //  Private variables
    private float _timer = 0f;
    private float _targetTime = 0f;
    private List<Letter> _letters = new List<Letter>();
    private Color _targetColor;

    public void ChangeDialogue(String text, Color? color = null)
    {
        StartCoroutine(ChangeTextRoutine(text, color));
    }

    public void SkipCurrentDialogue()
    {
        _timer = _targetTime;
        foreach (Letter letter in _letters)
        {
            letter.Transform.localScale = Vector3.one;
            letter.Transform.position = letter.StartPosition;
            letter.Text.color = _targetColor;
        }
    }

    public bool IsFinished()
    {
        return _timer >= _targetTime;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (!IsFinished()) UpdateText();
    }

    private void UpdateText()
        // We should lerp between no text and the target text using the time variables to decrease frame-timing errors
    {
        int i = (int)Mathf.Floor((_timer / _targetTime) * _letters.Count);
        Letter letter = _letters[i];

        // Animate current letter
        float t = (_timer / _targetTime) * _letters.Count - i;
        letter.Transform.localScale = Vector3.LerpUnclamped(Vector3.one * letterScaleEffect, Vector3.one , t);
        letter.Transform.position = Vector2.LerpUnclamped(letter.StartPosition + Vector2.up * letterMoveEffect, letter.StartPosition, t);
        letter.Text.color = Color.LerpUnclamped(new Color(_targetColor.r,  _targetColor.g, _targetColor.b, 0f), _targetColor, t);

        // Always make sure to complete the previous letter
        Letter previousLetter = _letters[i - 1];
        previousLetter.Transform.localScale = Vector3.one;
        previousLetter.Transform.position = letter.StartPosition;
        previousLetter.Text.color = _targetColor;
    }


    private IEnumerator ChangeTextRoutine(String targetText, Color? color = null)
    {
        float timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            
            // Fadeout
            foreach (Letter letter in _letters)
            {
                float t = fadeOutCurve.Evaluate(Mathf.Clamp01(timer / fadeOutTime));
                
                // Execute animation
                letter.Transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.one * letterScaleEffect, t);
                letter.Transform.position = Vector2.LerpUnclamped(letter.StartPosition, letter.StartPosition + Vector2.up * letterMoveEffect, t);
                letter.Text.color = Color.LerpUnclamped(_targetColor, new Color(_targetColor.r,  _targetColor.g, _targetColor.b, 0f), t);
            }
            
            yield return null;
        }
        // Reset
        RemoveAllLetters();
        
        // Append
        _targetColor = color ?? defaultTextColor;
        _targetTime = (targetText.Length + 1f) * characterTime;
        SpawnNewLetters(targetText);
    }

    private void RemoveAllLetters()
    {
        // Remove children
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        // Clear list
        _letters.Clear();
    }
    
    private void SpawnNewLetters(String text)
    {
        foreach (Char character in text)
        {
            // Instantiate
            GameObject  letterObject = Instantiate(letterPrefab, textParent);
            Transform letterTransform = letterObject.transform;
            TextMeshPro textMesh = letterObject.GetComponent<TextMeshPro>();
            
            // Set text
            textMesh.text = character.ToString();
            
            // Add to list
            Letter letter = new Letter();
            letter.Transform = letterTransform;
            letter.Text = textMesh;
            letter.StartPosition = letterTransform.position;
            _letters.Add(letter);
            
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(textParent.GetComponent<RectTransform>());
        
        foreach (Letter letter in _letters)
        {
            // Set transform & color
            letter.Transform.localScale = Vector3.one * letterScaleEffect;
            letter.Transform.position = letter.StartPosition +  Vector2.up * letterMoveEffect;
            letter.Text.color = new Color(_targetColor.r, _targetColor.g, _targetColor.b, 0f);
        }
    }
}
