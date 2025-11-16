using UnityEngine;

public class Paintable : MonoBehaviour
{
    public int textureSize;
    private Texture2D texture;
    private Renderer renderer;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        texture = new Texture2D(textureSize, textureSize);
        renderer.material.mainTexture = texture;
    }

    //https://docs.unity3d.com/ScriptReference/RaycastHit.html
    //raycast antaa uv texture cordin johon piirretään
    public void Paint(Vector2 uv, Color color, float size)
    {
        int cx = (int)(uv.x * textureSize); //skaalaus
        int cy = (int)(uv.y * textureSize); //skaalaus
        int brush_diameter = Mathf.RoundToInt(size * textureSize);
        //piirretään ympyrä, nii katsotaan koordinaatin ympäri!
        for (int x = -brush_diameter; x <= brush_diameter; x++)
        {
            for (int y = -brush_diameter; y <= brush_diameter; y++)
            {
                if (x * x + y * y > brush_diameter * brush_diameter) continue; //ilman tätä tulee neliö, siistii jos haluu tehä pixel art
                int px = cx + x;
                int py = cy + y;
                if (px < 0 || px >= textureSize || py < 0 || py >= textureSize)
                    continue;
                texture.SetPixel(px, py, color);
            }
        }
        texture.Apply();
    }
}
