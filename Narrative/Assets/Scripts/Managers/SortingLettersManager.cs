using UnityEngine;

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
}
