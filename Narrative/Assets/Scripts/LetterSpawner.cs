using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LetterSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2Int randomAmount =  Vector2Int.zero;
    [SerializeField] private Vector2 randomRotation = Vector2.zero;
    [Header("References")]
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private Transform stack1;
    [SerializeField] private Transform stack2;

    private void Awake()
    {
        for (int i = 0; i < Random.Range(randomAmount.x, randomAmount.y); i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(randomRotation.x, randomRotation.y));
        }
    }

    private void Update()
    {
        if (transform.childCount == 0) return;
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveChild(1);
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveChild(2);
    }

    private void MoveChild(int targetNum)
    {
        Transform target = targetNum == 1 ? stack1 : stack2;
        Transform child = transform.GetChild(0);
        Letter letter = child.GetComponent<Letter>();
        child.SetParent(target);
        letter.MoveToTarget(target.position);
        if (letter.correctStack == targetNum) Good();
        else Bad();
    }

    private void Good()
    {
        SortingLettersManager.Instance.correctLetters++;
        print("Good");
    }

    private void Bad()
    {
        SortingLettersManager.Instance.wrongLetters++;
        print("Bad");
    }
}
