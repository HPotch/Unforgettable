using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private float endFadeTime = 0.2f;
    [SerializeField] private float loopTime = 2f;
    

    private int _currentIndex = 0;

    private void Start()
    {
        PlayClip(audioClips[_currentIndex]);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (_currentIndex >= audioClips.Count) return;
            AudioManager.Instance.StopAllAudio(audioClips[_currentIndex]);
            _currentIndex++;
            if (_currentIndex >= audioClips.Count) return;
            PlayClip(audioClips[_currentIndex]);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        AudioManager.Instance.PlayAudio(clip, 1f, 1f, 0f, endFadeTime, loopTime);
    }
}
