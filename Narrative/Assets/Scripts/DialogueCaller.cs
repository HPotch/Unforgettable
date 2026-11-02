using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DialogueComponent = DialogueManager.DialogueComponent;
using AudioSettings = AudioManager.AudioSettings;

public class DialogueCaller : MonoBehaviour
{
    public static DialogueCaller Instance;
    
    [Header("References")]
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] public List<DialogueComponent> dialogues = new List<DialogueComponent>();
    [SerializeField] private Transform text;
    [SerializeField] private AudioSettings writing;
    
    [Header("Settings")]
    [SerializeField] private float endFadeTime = 0.2f;
    [SerializeField] private float loopTime = 2f;
    [SerializeField] private float startWaitTime = 0f;
     
    // Private variables
    private int _audioIndex = 0;
    public int _dialogueIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void NextAudio()
    {
        if (_audioIndex >= audioClips.Count) return;
        AudioManager.Instance.StopAllAudio(audioClips[_audioIndex]);
        _audioIndex++;
        if (_audioIndex >= audioClips.Count) return;
        PlayClip(audioClips[_audioIndex]);
    }

    public void NextDialogue()
    {
        if (_dialogueIndex >= dialogues.Count)
        {
            Transition.Instance.StartDefaultTransition();
            return;
        }
        if (dialogues[_dialogueIndex].customNext != -1) _dialogueIndex = dialogues[_dialogueIndex].customNext;
        DialogueManager.Instance.ChangeDialogue(dialogues[_dialogueIndex]);
        _dialogueIndex++;
    }

    public void SetDialogue(int index)
    {
        if (index >= dialogues.Count)
        {
            Debug.LogError($"Dialogue Index: {index} out of range");
            return;
        }

        DialogueManager.Instance.Complete();
        DialogueManager.Instance.Exiting = false;
        DialogueManager.Instance.ChangeDialogue(dialogues[index]);
        _dialogueIndex = index;
    }

    private void Start()
    {
        if (_audioIndex < audioClips.Count) PlayClip(audioClips[_audioIndex]);
        DialogueManager.Instance.onTextEnter.AddListener(PlayWritingAudio);
        StartCoroutine(StartWait());
        DialogueManager.Instance.onTextEnter.AddListener(PlayWritingAudio);
    }
    
    private IEnumerator StartWait()
    {
        yield return new WaitForSeconds(startWaitTime);
        DialogueManager.Instance.ChangeDialogue(dialogues[_dialogueIndex]);
        _dialogueIndex++;
    }

    private void PlayWritingAudio()
    {
        DialogueComponent dc = DialogueManager.Instance.TargetDC;
        if (dc.text is "" or " " or "  " || dc.dontPlayAudio) return;
        AudioManager.Instance.PlayAudioSettings(writing);
    }

    private void Update()
    {
        DialogueManager dm = DialogueManager.Instance;
        DialogueComponent dc = dm.TargetDC;

        if (dc == null) return;
        if (dc.skipState == DialogueManager.SkipState.NoSkip) return;
        if (dm.Exiting) return;
        bool timeSkip = dm.Completed && dc.autoSkipTime > 0f && dm.TimeSinceText > dc.autoSkipTime;
        bool inputSkip = (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && dc.skipState == DialogueManager.SkipState.Skippable;
        if (!(inputSkip || timeSkip)) return;

        if (dm.Completed) NextDialogue();
        else dm.Complete();
    }

    private void PlayClip(AudioClip clip)
    {
        AudioManager.Instance.PlayAudio(clip, 1f, 1f, 0f, endFadeTime, loopTime);
    }
}
