using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width = 4;
    [SerializeField] private float spacing = 20f;
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private bool update = false;

    private void Awake()
    {
        UpdateGrid();
    }

    private void Update()
    {
        if (update) UpdateGrid();
    }
    
    private void UpdateGrid()
    {
        ClearChildren();
        Vector2 center = Vector2.one * ((spacing * (width - 1)) * 0.5f);
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject square = Instantiate(squarePrefab, transform);
                square.transform.position = new Vector2(spacing * x, spacing * y) - center;
            }
        }
    }

    private void ClearChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }
}
