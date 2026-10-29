using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;

    public void Spawn(int i)
    {
        Instantiate(prefabs[i], transform);
    }
}
