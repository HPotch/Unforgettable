using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DialogueComponent = DialogueManager.DialogueComponent;

public class Test : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<DialogueComponent> dialogues = new List<DialogueComponent>();
    
    // Private variables
    private int _index = 0;

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        DialogueManager dm = DialogueManager.Instance;
        if (dm.exiting) return;

        if (dm.completed)
        {
            dm.ChangeDialogue(dialogues[_index]);
            _index++;
            if (_index >= dialogues.Count) _index = 0;
        }
        else
            dm.Complete();
    }
}
