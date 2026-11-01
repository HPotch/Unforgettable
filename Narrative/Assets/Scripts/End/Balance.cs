using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxRot;

    private void Update()
    {
        transform.eulerAngles = new Vector3(
            0, 0,
            Mathf.Lerp(maxRot, -maxRot, slider.Value));
    }
}
