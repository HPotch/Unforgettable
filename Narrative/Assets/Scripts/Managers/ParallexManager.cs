using UnityEngine;

public class ParallexManager : MonoBehaviour
{
    public static ParallexManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Vector2 position = Vector2.zero;
}
