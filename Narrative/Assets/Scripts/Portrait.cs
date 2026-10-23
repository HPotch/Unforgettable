using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    private static readonly int T = Shader.PropertyToID("_T");
    [SerializeField] private Dictionary<string, Sprite> portraits;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float animationSpeed = 0.5f;
    [SerializeField] private AnimationCurve animationCurve;

    private Image _image;
    private Material _mat;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _mat = _image.material;
        _mat.SetFloat(T, 0f);
    }
    
    public void ShowPortrait(int i = 0)
    {
        if (portraits.Values.ToList()[i] == _image.sprite && _mat.GetFloat(T) > 0f) return; // Return if switching to the same portrait and not hidden
        StartCoroutine(ShowPortraitRoutine(i));
    }
    
    public void HidePortrait()
    {
        StartCoroutine(VisibilityAnimation(false));
    }

    public void ChangeName(int i = 0, string name = "")
    {
        portraits.Keys.ToList()[i] = name;
    }
    
    
    private IEnumerator ShowPortraitRoutine(int i = 0)
    {
        // Hide portrait first, if needed
        if (_mat.GetFloat(T) > 0f)
        {
            HidePortrait();
            yield return new WaitForSeconds(animationSpeed);
            ChangePortrait(i);
            yield break;
        }
        ChangePortrait(i);
    }
    
    private IEnumerator VisibilityAnimation(bool show = true)
    {
        text.enabled = show;
        float timer = 0f;

        while (timer < animationSpeed)
        {
            timer += Time.deltaTime;
            float t = animationCurve.Evaluate(timer / animationSpeed);
            _mat.SetFloat(T, show ? t : 1 - t);
            yield return null;
        }
        // Exit cleanly
        _mat.SetFloat(T, show ? 1 : 0);
    }
    
    private void ChangePortrait(int i = 0)
    {
        // Change
        if (portraits.Count < i)
        {
            Debug.LogError($"Couldn't load to portrait {i} - no entry found, aborting");
            return;
        }
        _image.sprite = portraits.Values.ToList()[i];
        text.text = portraits.Keys.ToList()[i];
        // Animate
        StartCoroutine(VisibilityAnimation(true));
    }
}
