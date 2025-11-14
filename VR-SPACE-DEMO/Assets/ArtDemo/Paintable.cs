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

    public void Paint(Vector2 uv, Color color, float size)
    {
        int cx = (int)(uv.x * textureSize);
        int cy = (int)(uv.y * textureSize);
        int r = Mathf.RoundToInt(size * textureSize);
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y > r * r) continue;
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
