using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    // Manager
    public static Transition Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    [Header("Settings")]
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private float targetScale = 35f;
    [SerializeField] private bool transitionIn = true;
    [SerializeField] private SceneAsset defaultScene;
    private RayManager ray;
    
    // Private variables
    private string _targetScene;
    private Coroutine _routine;
    private readonly Dictionary<int, string> _buildScenes = new Dictionary<int, string>();

    private void Start()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string s = SceneUtility.GetScenePathByBuildIndex(i);
            _buildScenes[i] = s;
        }
        
        transform.localScale = Vector3.one * targetScale;
        if (transitionIn) StartCoroutine(TransitionRoutine(false));
        else transform.localScale = Vector3.zero;
    }
    
    public void StartDefaultTransition()
    {
        StartTransition(defaultScene);
    }
    
    public void StartTransition(SceneAsset nextScene)
    {
        // Errors
        if (!sceneInBuild(nextScene.name))
        {
            Debug.LogError($"Scene '{nextScene.name}' is not valid or not in Build Settings!");
            return;
        }
        if (_routine != null)
        {
            Debug.LogWarning("Already transitioning, aborting"); // Damn I feel like secret space agent using the word "aborting"!
            return;
        }
        // Transition
        _targetScene = nextScene.name;
        _routine = StartCoroutine(TransitionRoutine());
    }

    private bool sceneInBuild(string s)
    {
        foreach (string scene in _buildScenes.Values)
        {
            if (scene.Contains(s)) return true;
        }
        return false;
    }

    private IEnumerator TransitionRoutine(bool forward = true)
    {
        float timer = 0f;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float sample = transitionCurve.Evaluate(timer / transitionTime);
            
            // Animate
            if (forward) transform.localScale = Vector3.one * Mathf.Lerp(0f, targetScale, sample);
            else transform.localScale = Vector3.one * Mathf.Lerp(targetScale, 0f, sample);
            
            yield return null;
        }
        
        // Exit cleanly
        _routine = null;
        transform.localScale = Vector3.one * (targetScale * (forward ? 1f : 0f));
        
        if (forward && _targetScene != "") SceneManager.LoadScene(_targetScene); // Switch scene
    }
}
