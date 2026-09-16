using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DialogueComponent = DialogueManager.DialogueComponent;

public class IntroScene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private List<DialogueComponent> dialogues = new List<DialogueComponent>();
    [SerializeField] private Sprite[] eyeSprites;
    [SerializeField] private SpriteRenderer[] eyeRenderers;
    [Header("Settings")]
    [SerializeField] private float endFadeTime = 0.2f;
    [SerializeField] private float loopTime = 2f;
    [SerializeField] private float eyeTime = 2.5f;
    
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

    public void OpenEyes()
    {
        StartCoroutine(OpenEyeRoutine());
    }

    private IEnumerator OpenEyeRoutine()
    {
        float time = 0f;

        while (time < eyeTime)
        {
            time += Time.deltaTime;

            int i = (int)Mathf.Floor((time / eyeTime) * eyeSprites.Length - 1);
            i = Mathf.Clamp(i, 0, eyeSprites.Length);
            foreach (SpriteRenderer eyeRenderer in eyeRenderers)
                eyeRenderer.sprite = eyeSprites[i];
            
            yield return null;
        }
    }

    private void Awake()
    {
        foreach (SpriteRenderer eyeRenderer in eyeRenderers)
            eyeRenderer.sprite = eyeSprites[0];
    }
    
    private void Start()
    {
        PlayClip(audioClips[_audioIndex]);
        DialogueManager.Instance.ChangeDialogue(dialogues[_dialogueIndex]);
        _dialogueIndex++;
    }

    private void Update()
    {
        DialogueManager dm = DialogueManager.Instance;
        
        bool timeSkip = dm.Completed && dm.TargetDC?.autoSkipTime > 0f && dm.TimeSinceText > dm.TargetDC?.autoSkipTime;

        if (dm.Exiting || (!Keyboard.current.spaceKey.wasPressedThisFrame && !timeSkip)) return;

        if (dm.Completed)
        {
            dm.ChangeDialogue(dialogues[_dialogueIndex]);
            _dialogueIndex++;
            if (_dialogueIndex >= dialogues.Count) _dialogueIndex = 0;
        }
        else
            dm.Complete();
    }

    private void PlayClip(AudioClip clip)
    {
        AudioManager.Instance.PlayAudio(clip, 1f, 1f, 0f, endFadeTime, loopTime);
    }
}
