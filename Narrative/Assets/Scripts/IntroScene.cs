using System.Collections.Generic;
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
    [SerializeField] private Animator bottomLid;
    [SerializeField] private Animator topLid;
    [SerializeField] private Transform text;
    [SerializeField] private AudioSettings rain;
    
    [Header("Settings")]
    [SerializeField] private float endFadeTime = 0.2f;
    [SerializeField] private float loopTime = 2f;
    
     
    // Private variables
    private int _audioIndex = 0;
    private int _dialogueIndex = 0;
    

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
        DialogueManager.Instance.ChangeDialogue(dialogues[_dialogueIndex]);
        _dialogueIndex++;
        if (_dialogueIndex >= dialogues.Count) _dialogueIndex = 0;
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

    private void Start()
    {
        PlayClip(audioClips[_audioIndex]);
        DialogueManager.Instance.ChangeDialogue(dialogues[_dialogueIndex]);
        AudioManager.Instance.PlayAudioSettings(rain);
        _dialogueIndex++;
    }

    private void Update()
    {
        DialogueManager dm = DialogueManager.Instance;

        if (dm.TargetDC.skipState == DialogueManager.SkipState.NoSkip) return;
        if (dm.Exiting) return;
        bool timeSkip = dm.Completed && dm.TargetDC?.autoSkipTime > 0f && dm.TimeSinceText > dm.TargetDC?.autoSkipTime;
        bool keyboardSkip = Keyboard.current.spaceKey.wasPressedThisFrame && dm.TargetDC.skipState == DialogueManager.SkipState.Skippable;
        if (!(keyboardSkip || timeSkip)) return;

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
