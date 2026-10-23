using System.Collections;
using UnityEngine;

public class DaughterScene : MonoBehaviour
{
    [Header("References")] [Header("Settings")] [SerializeField]
    private Vector2 puzzleCamPos = Vector2.right * 200f;
    [SerializeField] private float puzzleCamSpeed = 1f;
    
    public void StartMiniGame()
    {
        ParallexManager.Instance.SetPosition(puzzleCamPos, puzzleCamSpeed);
    }

    public void QuitMiniGame()
    {
    }

}
