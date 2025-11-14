using UnityEngine;

public class ColorSwap : Interactive
{

    public Material color;
    public MeshRenderer brushMeshRenderer;


    
    public new void Interact()
    {
        brushMeshRenderer.material = color;  
    }
}
