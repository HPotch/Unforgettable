using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using AudioSettings = AudioManager.AudioSettings;
using Random = UnityEngine.Random;

public class BlurMiniGame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioSettings sliderAudio;
    [SerializeField] private float blurStrength;
    [SerializeField] private float snapPercent = 5f;
    [FormerlySerializedAs("SliderEnterCurve")] [SerializeField] private AnimationCurve sliderEnterCurve;
    [SerializeField] private float sliderEnterTime = 1f;
    [SerializeField] private float sliderDuckOnHide = 5f;
    [SerializeField] private UnityEvent OnCompleted;
    [Header("References")]
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    [SerializeField] private Blur front;
    [SerializeField] private Blur back;
    [SerializeField] private RectTransform sliderParent;
    
    // Private variables
    private float blurAmount;

    private Vector2 blurTargets;
    private float _showYPos;

    public void StartMiniGame()
    {
        slider1.enabled = true;
        slider2.enabled = true;
        StartCoroutine(AnimationRoutine(true));
    }

    private void StopMiniGame()
    {
        slider1.enabled = false;
        slider2.enabled = false;
        StartCoroutine(AnimationRoutine(false));
    }

    private IEnumerator AnimationRoutine(bool forward = true)
    {
        float timer = forward ? 0f : sliderEnterTime;

        while (forward ? timer < sliderEnterTime : timer > 0f)
        {
            timer += Time.deltaTime * (forward ? 1f : -1f);
            float t = sliderEnterCurve.Evaluate(Mathf.Clamp01(timer / sliderEnterTime));
            sliderParent.anchoredPosition = new Vector2(sliderParent.anchoredPosition.x, Mathf.Lerp(_showYPos - sliderDuckOnHide, _showYPos, t));
            
            yield return null;
        }
    }
    
    private void Awake()
    {
        slider1.snapPercent = snapPercent;
        slider2.snapPercent = snapPercent;
        blurTargets = new Vector2(
            Mathf.Round((Random.Range(0f, 1f) * 100f) / snapPercent) * snapPercent * 0.01f,
            Mathf.Round((Random.Range(0f, 1f) * 100f) / snapPercent) * snapPercent * 0.01f);
        
        slider1.OnValueChanged.AddListener(Changed);
        slider2.OnValueChanged.AddListener(Changed);
        slider1.SetValue(Mathf.Repeat(blurTargets.x + 0.5f, 1f));
        slider2.SetValue(Mathf.Repeat(blurTargets.y + 0.5f, 1f));
        UpdateValue();

        _showYPos = sliderParent.anchoredPosition.y;
        StopMiniGame();
    }

    private void Changed()
    {
        AudioManager.Instance.PlayAudioSettings(sliderAudio);
        UpdateValue();
    }

    private void UpdateValue()
    {
        blurAmount = Mathf.Abs(slider1.Value - blurTargets.x) + Mathf.Abs(slider2.Value - blurTargets.y);
        
        front.SetBlur(blurAmount * blurStrength);
        back.SetBlur((1f - blurAmount) * blurStrength);

        if (Mathf.Approximately(slider1.Value, blurTargets.x) && Mathf.Approximately(slider2.Value, blurTargets.y))
        {
            OnCompleted.Invoke();
            StopMiniGame();
        }
    }
}
