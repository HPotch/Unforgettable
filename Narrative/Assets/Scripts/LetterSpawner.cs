using UnityEngine;
using Random = UnityEngine.Random;

public class LetterSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2Int randomAmount =  Vector2Int.zero;
    [SerializeField] private Vector2 randomRotation = Vector2.zero;
    [Header("References")]
    [SerializeField] private GameObject letterPrefab;

    private void Awake()
    {
        for (int i = 0; i < Random.Range(randomAmount.x, randomAmount.y); i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(randomRotation.x, randomRotation.y));
        }
    }
    
}
