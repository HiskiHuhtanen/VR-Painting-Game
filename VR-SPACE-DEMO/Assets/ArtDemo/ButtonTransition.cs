using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonTransition : MonoBehaviour
{
    [Header("Scene Transition")]
    public string hallwaySceneName = "Hallway";
    public bool useSceneIndex = false;
    public int hallwaySceneIndex = 1; 
    
    [Header("Optional Feedback")]
    public AudioSource audioSource;
    public AudioClip transitionSound;
    public float transitionDelay = 0.5f;
    
    [Header("Button Settings")]
    public string buttonTag = "Button";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(buttonTag))
        {
            StartTransition();
        }
    }

    void StartTransition()
    {
        if (audioSource && transitionSound)
        {
            audioSource.clip = transitionSound;
            audioSource.Play();
        }

        if (transitionDelay > 0)
        {
            Invoke(nameof(LoadHallwayScene), transitionDelay);
        }
        else
        {
            LoadHallwayScene();
        }
        
    }

    void LoadHallwayScene()
    {
        if (useSceneIndex)
        {
            SceneManager.LoadScene(hallwaySceneIndex);
        }
        else
        {
            SceneManager.LoadScene(hallwaySceneName);
        }
    }

    void OnValidate()
    {
        if (Application.isPlaying) return;
        
        if (string.IsNullOrEmpty(hallwaySceneName))
            hallwaySceneName = "Hallway";
    }
}
