using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SortingLettersManager : MonoBehaviour
{
    public static SortingLettersManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public UnityEvent afterMove;
    public bool end = false;
    [SerializeField] private UnityEvent onEnd;
    [SerializeField] private GameObject controls;
    [SerializeField] private Transform letterSpawner;
    [SerializeField] private Transform stack1, stack2;
    [SerializeField] private UnityEvent onSpamSpam, onSpamImportant, onImportantSpam, onImportantImportant;
    [SerializeField] private Vector2Int spamSpam, spamImportant, importantSpam, importantImportant; // Ranges, so x = min, y = max
    [SerializeField] private int wrongOnLieIndex;
    [SerializeField] private Hare hare;

    private float center = 0f;

    private void Start()
    {
        center = (stack1.position.x + stack2.position.x) / 2f; // Calculate center between stack 1 and 2
    }

    // 0 = left, 1 = right
    public int CurrentDirection()
    {
        return letterSpawner.GetChild(0).GetComponent<Letter>().correctStack;
    }
    
    private void Update()
    {
        if (end) return;
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveChild(0);
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveChild(1);
        else if (Mouse.current.leftButton.wasPressedThisFrame)
            MoveChild(RayManager.Instance.mouseWorldPos.x < center ? 0 : 1);
        else return;
        Destroy(controls); // User has inputted, we can remove the help
    }
    
    private void MoveChild(int targetNum)
    {
        Transform target = targetNum == 0 ? stack1 : stack2;
        Transform child = letterSpawner.GetChild(0);
        Letter letter = child.GetComponent<Letter>();
        child.SetParent(target);
        letter.MoveToTarget(target.position);
        if (letter.correctStack == targetNum) Good(targetNum);
        else Bad(targetNum, hare.lying);
    }
    
    private void Good(int num)
    {
        // Simulate a bad move when the hare is lying and somehow the player did well
        if (hare.lying)
        {
            Bad(num, false);
            return;
        }
        
        UnityEvent ue = num == 0 ? onSpamSpam : onImportantImportant;
        ue.Invoke();
        
        Vector2Int range = num == 0 ? spamSpam : importantImportant;
        
        DialogueCaller.Instance.SetDialogue(Random.Range(range.x, range.y + 1));
    }

    private void Bad(int num, bool lying = false)
    {
        UnityEvent ue = num == 0 ? onImportantSpam : onSpamImportant;
        ue.Invoke();
        
        Vector2Int range = num == 0 ? importantSpam : spamImportant; // Get correct range
        int i = Random.Range(range.x, range.y + 1); // Calculate index
        
        // Game end
        if (lying)
        {
            i = wrongOnLieIndex; // Set to lieIndex if the hare is lying
            end = true;
            onEnd.Invoke();
        }
        
        DialogueCaller.Instance.SetDialogue(i);
    }
}
