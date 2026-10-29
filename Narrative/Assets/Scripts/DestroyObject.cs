using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    
    public void Execute()
    {
        Destroy(objectToDestroy == null ? gameObject : objectToDestroy);
    }
}
