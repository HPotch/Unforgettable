using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using DialogueComponent = DialogueManager.DialogueComponent;
using AudioSettings = AudioManager.AudioSettings;

public class IntroScene : MonoBehaviour
{
    private static readonly int OpenEye = Animator.StringToHash("OpenEye");

    [Header("References")]
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] public List<DialogueComponent> dialogues = new List<DialogueComponent>();
    [SerializeField] private Animator bottomLid, topLid;
    [SerializeField] private Transform text;
    [SerializeField] private AudioSettings rain, writing, coffee;
    [SerializeField] private SpriteRenderer heart;
    
    [Header("Settings")]
    [SerializeField] private float endFadeTime = 0.2f;
    [SerializeField] private float loopTime = 2f;
    [SerializeField] private float startWaitTime = 3f;
     
    // Private variables
    private int _audioIndex = 0;
    public int _dialogueIndex = 0;

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
        DialogueManager.Instance.ChangeDialogue(dialogues[_dialogueIndex]);
        _dialogueIndex++;
    }

    public void OpenEyes()
    {
        bottomLid.SetTrigger(OpenEye);
        topLid.SetTrigger(OpenEye);
    }

    public void MoveText()
    {
        text.position = new Vector2(0f, -4.35f);
    }

    public void ShowHeart()
    {
        heart.enabled = true;
    }
    
    public void HideHeart()
    {
        heart.enabled = false;
    }

    private void Start()
    {
        PlayClip(audioClips[_audioIndex]);
        AudioManager.Instance.PlayAudioSettings(rain);
        AudioManager.Instance.PlayAudioSettings(coffee);
        StartCoroutine(StartWait());
        DialogueManager.Instance.onTextEnter.AddListener(PlayWritingAudio);
    }

    private IEnumerator StartWait()
    {
        float timer = 0f;
        while (timer < startWaitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        DialogueManager.Instance.ChangeDialogue(dialogues[_dialogueIndex]);
        _dialogueIndex++;
    }
    
    private void PlayWritingAudio()
    {
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

        if (dm.Completed)
            NextDialogue();
        else
            dm.Complete();
    }

    private void PlayClip(AudioClip clip)
    {
        AudioManager.Instance.PlayAudio(clip, 1f, 1f, 0f, endFadeTime, loopTime);
    }
}
