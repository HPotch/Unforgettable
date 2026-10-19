using UnityEngine;
using UnityEngine.InputSystem;

public class SortingLettersManager : MonoBehaviour
{
    public static SortingLettersManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int correctLetters = 0;
    public int wrongLetters = 0;

    [SerializeField] private GameObject controls;
    [SerializeField] private Transform letterSpawner;
    [SerializeField] private Transform stack1, stack2;

    // 0 = left, 1 = right
    public int CurrentDirection()
    {
        return letterSpawner.GetChild(0).GetComponent<Letter>().correctStack;
    }
    
    private void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveChild(1);
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveChild(2);
        else return;
        Destroy(controls); // User has inputted, we can remove the help
    }
    
    private void MoveChild(int targetNum)
    {
        Transform target = targetNum == 1 ? stack1 : stack2;
        Transform child = letterSpawner.GetChild(0);
        Letter letter = child.GetComponent<Letter>();
        child.SetParent(target);
        letter.MoveToTarget(target.position);
        if (letter.correctStack == targetNum) Good();
        else Bad();
    }
    
    private void Good()
    {
        correctLetters++;
        print("Good");
    }

    private void Bad()
    {
        Instance.wrongLetters++;
        print("Bad");
    }
}
