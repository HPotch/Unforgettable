using System;
using UnityEngine;

public class GridPart : MonoBehaviour
{
    [NonSerialized] public bool Filled = false;

    private SpriteRenderer _sr;
    private Color _startColor;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _startColor = _sr.color;
    }

    private void Update()
    {
        _sr.color = Filled ? Color.green : _startColor;
    }
}
