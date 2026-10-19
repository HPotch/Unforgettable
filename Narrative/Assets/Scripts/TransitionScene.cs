using EasyTextEffects;
using UnityEngine;

public class TransitionScene : MonoBehaviour
{
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private TextEffect effect;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < waitTime) return;
        effect.StartManualEffect("ColorOut");
        effect.StartManualEffect("ScaleOut");
        effect.StartManualEffect("MoveOut");
        Destroy(gameObject);
    }
}
