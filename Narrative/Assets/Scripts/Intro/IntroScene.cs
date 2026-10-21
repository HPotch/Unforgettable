using UnityEngine;
using AudioSettings = AudioManager.AudioSettings;

public class IntroScene : MonoBehaviour
{
    private static readonly int OpenEye = Animator.StringToHash("OpenEye");

    [Header("References")]
    [SerializeField] private Animator bottomLid, topLid;
    [SerializeField] private Transform text;
    [SerializeField] private AudioSettings rain, coffee;
    [SerializeField] private SpriteRenderer heart;

    public void OpenEyes()
    {
        bottomLid.SetTrigger(OpenEye);
        topLid.SetTrigger(OpenEye);
    }

    public void MoveText()
    {
        DialogueManager.Instance.textObject = text;
        DialogueManager.Instance.SetTextVariables();
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
        AudioManager.Instance.PlayAudioSettings(rain);
        AudioManager.Instance.PlayAudioSettings(coffee);
    }
}
