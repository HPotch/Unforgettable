using UnityEngine;
using AudioSettings = AudioManager.AudioSettings;

public class IntroScene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform text;
    [SerializeField] private AudioSettings rain, coffee;
    [SerializeField] private SpriteRenderer heart;

    public void MoveText()
    {
        Destroy(DialogueManager.Instance.textObject.gameObject);
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
