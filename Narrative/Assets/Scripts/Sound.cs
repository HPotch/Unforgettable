using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using AudioSettings = AudioManager.AudioSettings;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSettings audio;
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private float waitTime = 0f;
    public UnityEvent onPlay;

    private void Start()
    {
        if (playOnStart) StartCoroutine(WaitAndSound());
    }

    public void Execute()
    {
        StartCoroutine(WaitAndSound());
    }

    private IEnumerator WaitAndSound()
    {
        yield return new WaitForSeconds(waitTime);
        onPlay.Invoke();
        AudioManager.Instance.PlayAudioSettings(audio);
    }
}
