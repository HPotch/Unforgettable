using UnityEngine;

public class Blur : MonoBehaviour
{
    private Material _material;

    public void SetBlur(float value)
    {
        if (!_material) _material = GetComponent<SpriteRenderer>().material;
        _material.SetFloat("_Radius", value);
    }
}
