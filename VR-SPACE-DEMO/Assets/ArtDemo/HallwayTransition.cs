using UnityEngine;
using UnityEngine.SceneManagement;

public class HallwayTransition : Interactive
{
    [Header("Scene Transition")]
    public string hallwaySceneName = "HallwayScene";
    public bool useSceneIndex = false;
    public int hallwaySceneIndex = 1;
    
    [Header("Optional Feedback")]
    public AudioSource audioSource;
    public AudioClip transitionSound;
    public float transitionDelay = 0.5f;

    public new void Interact()
    {
        StartTransition();
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
        
        Debug.Log("Transitioning to hallway scene...");
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
        // Helper to show scene info in inspector
        if (Application.isPlaying) return;
        
        if (string.IsNullOrEmpty(hallwaySceneName))
            hallwaySceneName = "HallwayScene";
    }
}