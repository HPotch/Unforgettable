using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform wrongPart1, rightPart, wrongPart2, balance;
    [SerializeField] private Fade[] normals, bunnies;

    [SerializeField] private float rightPartWidth = 100f, requiredTime = 2f;
    [SerializeField] private UnityEvent onWon;

    private float _value = 0f;
    
    private void Awake()
    {
        float totalWidth = balance.transform.localScale.x;
        float wrongPartWidth = (totalWidth - rightPartWidth) / 2f;
        wrongPart1.localScale = new Vector3(wrongPartWidth, wrongPart1.localScale.y, wrongPart1.localScale.z);
        wrongPart1.localPosition = new Vector3((wrongPartWidth / 2f + rightPartWidth / 2f) * -1f, 0f, 0f);
        wrongPart2.localScale = new Vector3(wrongPartWidth, wrongPart2.localScale.y, wrongPart2.localScale.z);
        wrongPart2.localPosition = new Vector3(wrongPartWidth / 2f + rightPartWidth / 2f, 0f, 0f);
        rightPart.localScale = new Vector3(rightPartWidth, rightPart.localScale.y, rightPart.localScale.z);
    }

    private void Update()
    {
        if (transform.localPosition.x < rightPartWidth / 2f &&
            transform.localPosition.x > -rightPartWidth / 2f)
            _value += Time.deltaTime / requiredTime;
        else _value -= Time.deltaTime / requiredTime;
        _value = Mathf.Clamp(_value, 0f, 1f);
        
        foreach (Fade fade in normals)
            fade.SetFadeAmount(_value);
        foreach (Fade fade in bunnies)
            fade.SetFadeAmount(1 - _value);
        
        if (_value >= 1f)
            onWon.Invoke();
    }
}
