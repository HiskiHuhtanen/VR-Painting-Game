using UnityEngine;

public class ColorSwap : Interactive
{

    public Material color;
    public MeshRenderer brushMeshRenderer;
    public PaintBrush paintBrush;


    
    public new void Interact()
    {
        brushMeshRenderer.material = color;  
        paintBrush.SetColor(color.color);
    }
}
