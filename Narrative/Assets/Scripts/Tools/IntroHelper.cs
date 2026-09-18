using UnityEngine;
using DialogueComponent = DialogueManager.DialogueComponent;

[ExecuteInEditMode]
public class IntroHelper : MonoBehaviour
{
    [SerializeField] private bool resetAlpha;
    
    private IntroScene introScene;

    private void Awake()
    {
        introScene = GetComponent<IntroScene>();
    }

    private void Update()
    {
        if (!resetAlpha) return;
        foreach (DialogueComponent dc in introScene.dialogues)
        {
            dc.color.a = 1f;
        }
    }
}
