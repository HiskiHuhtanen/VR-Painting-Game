using UnityEngine;
using TMPro;
using System.Collections;

public class AppraisalDisplay : MonoBehaviour
{
    public TextMeshProUGUI textField;
    
    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip gearSound;
    public AudioClip coinSound;
    public AudioClip kaChingSound;

    public void ShowValue(float finalValue, System.Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateValue(finalValue, onComplete));
    }

    IEnumerator AnimateValue(float target, System.Action onComplete = null)
    {
        textField.text = "APPRAISING...";
        
        if (audioSource && gearSound)
        {
            audioSource.clip = gearSound;
            audioSource.Play();
            yield return new WaitForSeconds(gearSound.length);
        }
        else
        {
            yield return new WaitForSeconds(0.6f);
        }
        
        textField.text = "";
        yield return new WaitForSeconds(0.5f);

        if (audioSource && coinSound)
        {
            audioSource.clip = coinSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        
        float current = 0f;
        float duration = 1.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            t = Mathf.Pow(t, 2);

            current = Mathf.Lerp(0, target, t);
            textField.text = "$" + Mathf.FloorToInt(current).ToString("N0");
            
            if (audioSource && coinSound)
            {
                audioSource.pitch = Mathf.Lerp(0.7f, 1.3f, t);
            }

            yield return null;
        }
        

        if (audioSource && coinSound)
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.pitch = 1.0f;
        }
        textField.text = "$" + Mathf.FloorToInt(target).ToString("N0");
        if (audioSource && kaChingSound)
        {
            audioSource.clip = kaChingSound;
            audioSource.Play();
        }
        yield return new WaitForSeconds(3.0f);
        onComplete?.Invoke();
    }
}
