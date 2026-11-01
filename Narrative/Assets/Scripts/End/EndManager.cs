using System.Collections;
using Unity.Tutorials.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [SerializeField] private AnimateValue bottomFrame, leftFrame;
    [SerializeField] private float targetHeight, targetWidth, animationSpeed;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float darkTextWait = 2f, normalTextWait = 3f, eyeWaitTime = 2f, closeEyeWaitTime = 4f, switchToBeginTime = 3f;
    [SerializeField] private Transform darkTextObject, normalTextObject;
    [SerializeField] private SpriteAnimation[] eyeAnimations;

    private float _startWidth, _startHeight;

    private void Awake()
    {
        _startHeight = bottomFrame.GetComponent<RectTransform>().sizeDelta.y;
        _startWidth = leftFrame.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void StartMiniGame()
    {
        RectTransform bottomRect = bottomFrame.GetComponent<RectTransform>();
        bottomFrame.AnimateFloat(
            targetHeight, 
            animationSpeed,
            curve, 
            val => bottomRect.sizeDelta = new Vector2(bottomRect.sizeDelta.x, val), 
            bottomRect.sizeDelta.y);
        
        RectTransform leftRect = leftFrame.GetComponent<RectTransform>();
        leftFrame.AnimateFloat(
            targetWidth, 
            animationSpeed,
            curve, 
            val => leftRect.sizeDelta = new Vector2(val, leftRect.sizeDelta.y), 
            leftRect.sizeDelta.x);
    }
    
    public void StopMiniGame()
    {
        RectTransform bottomRect = bottomFrame.GetComponent<RectTransform>();
        bottomFrame.AnimateFloat(
            _startHeight, 
            animationSpeed,
            curve, 
            val => bottomRect.sizeDelta = new Vector2(bottomRect.sizeDelta.x, val), 
            bottomRect.sizeDelta.y);
        
        RectTransform leftRect = leftFrame.GetComponent<RectTransform>();
        leftFrame.AnimateFloat(
            _startWidth, 
            animationSpeed,
            curve, 
            val => leftRect.sizeDelta = new Vector2(val, leftRect.sizeDelta.y), 
            leftRect.sizeDelta.x);
    }

    public void CloseEyes()
    {
        DialogueManager.Instance.textObject = darkTextObject;
        DialogueManager.Instance.SetTextVariables();
        StartCoroutine(WaitAndContinue(darkTextWait));
        foreach (SpriteAnimation sa in eyeAnimations)
            sa.StartAnimationBackwards();
    }

    private IEnumerator WaitAndContinue(float time)
    {
        yield return new WaitForSeconds(time);
        DialogueCaller.Instance.NextDialogue();
    }

    public void OpenEyes()
    {
        DialogueManager.Instance.textObject = normalTextObject;
        DialogueManager.Instance.SetTextVariables();
        StartCoroutine(WaitAndContinue(normalTextWait));
        StartCoroutine(WaitAndAnimate(eyeWaitTime));
    }

    private IEnumerator WaitAndAnimate(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (SpriteAnimation sa in eyeAnimations)
            sa.StartAnimation();
    }

    public void WaitAndCloseEyes()
    {
        StartCoroutine(WaitAndCloseEyes(closeEyeWaitTime, switchToBeginTime));
    }

    private IEnumerator WaitAndCloseEyes(float time, float time2)
    {
        yield return new WaitForSeconds(time);
        foreach (SpriteAnimation sa in eyeAnimations)
            sa.StartAnimationBackwards();
        yield return new WaitForSeconds(time2);
        SceneManager.LoadScene("01 Intro Dialogue");
    }
}
