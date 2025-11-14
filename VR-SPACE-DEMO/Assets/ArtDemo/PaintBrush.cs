using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public Transform BrushTip;
    public float rayDistance = 0.1f;
    public float brushSize = 0.05f;
    public Color currentColor = Color.red;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(BrushTip.position, BrushTip.forward, out RaycastHit hit, rayDistance))
        {
            Paintable paintable = hit.collider.GetComponent<Paintable>();
            if (paintable != null)
            {
                Vector2 uv = hit.textureCoord;
                paintable.Paint(uv, currentColor, brushSize);
            }
        }
    }
}
