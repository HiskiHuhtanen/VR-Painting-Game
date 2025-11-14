using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public InteractMode interactModeOverride = InteractMode.None;
    public float twistMultiplierOverride = 0f;
    public float dwellTimerOverride = 0f;

    public Material outline;
    public Material[] defaultMat;
    private bool isHighlighted = false;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            defaultMat = meshRenderer.sharedMaterials;
    }


    //nonii, täällä siis kopioidaan vanhat materiaalit talteen, koska meshrender ei tykkää tyhjistä kohdista
    //tehdää sitä myös uusi materiaali kohta johon laitetaan outline materiaali
    public void Highlight()
    {
        if (isHighlighted) return;
        if (meshRenderer != null && outline != null)
        {
            var mats = meshRenderer.sharedMaterials;
            var newMats = new Material[mats.Length + 1];
            for (int i = 0; i < mats.Length; i++)
            {
                newMats[i] = mats[i];
            }
            newMats[mats.Length] = outline;
            meshRenderer.sharedMaterials = newMats;
            isHighlighted = true;
        }
        else
        {
            Debug.LogWarning("Jotakin jäi laittamatta!");
        }
    }


    public void Unhighlight()
    {
        if (!isHighlighted || meshRenderer == null || defaultMat == null) return;
        meshRenderer.sharedMaterials = defaultMat;
        isHighlighted = false;
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + transform.name);
    }
}
