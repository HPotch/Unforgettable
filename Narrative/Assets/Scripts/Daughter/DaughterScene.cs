using System.Collections;
using UnityEngine;

public class DaughterScene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnimateValue animateWidth;
    
    [Header("Settings")]
    [SerializeField] private Vector2 parallexPos = Vector2.right * 200f;
    [SerializeField] private float parallexSpeed = 1f, panelWidth = 50f;
    [SerializeField] private AnimationCurve curve;

    private Vector2 _parallexStartPos;
    private float _panelStartWidth;

    private void Start()
    {
        _parallexStartPos = ParallexManager.Instance.position;
        _panelStartWidth = animateWidth.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void StartMiniGame()
    {
        // Animate parallex manager
        ParallexManager pm = ParallexManager.Instance;
        pm.GetComponent<AnimateValue>().AnimateV2(
            parallexPos,
            parallexSpeed,
            curve,
            val => pm.position.x = val,
            val => pm.position.y = val,
            pm.position);
        
        // Animate frame
        RectTransform rect = animateWidth.GetComponent<RectTransform>();
        animateWidth.AnimateFloat(
            panelWidth, 
            parallexSpeed,
            curve, 
            val => rect.sizeDelta = new Vector2(val, rect.sizeDelta.y), 
            rect.sizeDelta.x);
    }

    public void QuitMiniGame()
    {
        ParallexManager pm = ParallexManager.Instance;
        pm.GetComponent<AnimateValue>().AnimateV2(
            _parallexStartPos,
            parallexSpeed,
            curve,
            val => pm.position.x = val,
            val => pm.position.y = val,
            pm.position);
        
        // Animate frame
        RectTransform rect = animateWidth.GetComponent<RectTransform>();
        animateWidth.AnimateFloat(
            _panelStartWidth, 
            parallexSpeed,
            curve, 
            val => rect.sizeDelta = new Vector2(val, rect.sizeDelta.y), 
            rect.sizeDelta.x);
        
        Portrait.Instance.ChangeName(3, "Eevi");

        StartCoroutine(WaitAndContine(parallexSpeed));
    }

    private IEnumerator WaitAndContine(float time)
    {
        yield return new WaitForSeconds(time);
        DialogueCaller.Instance.NextDialogue();
    }

}
