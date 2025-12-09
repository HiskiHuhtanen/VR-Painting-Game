using UnityEngine;
using System.IO;

public class PaintingFrame : MonoBehaviour
{
    public Renderer canvasRenderer;

    public void LoadPainting(string path)
    {
        if (!File.Exists(path)) return;

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        
        // Apply rotation based on frame's physical rotation
        Texture2D rotatedTex = GetCorrectlyRotatedTexture(tex);
        
        canvasRenderer.material.mainTexture = rotatedTex;
        DestroyImmediate(tex);
    }

    public void LoadBlankOrEndMessage()
    {
        Texture2D blank = new Texture2D(2, 2);
        blank.SetPixel(0, 0, Color.black);
        blank.SetPixel(1, 1, Color.black);
        blank.Apply();
        canvasRenderer.material.mainTexture = blank;
    }

    Texture2D RotateTexture90Clockwise(Texture2D original)
    {
        int w = original.width;
        int h = original.height;
        Texture2D rotated = new Texture2D(h, w, original.format, false);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                rotated.SetPixel(y, w - x - 1, original.GetPixel(x, y));
            }
        }
        rotated.Apply();
        return rotated;
    }

    Texture2D GetCorrectlyRotatedTexture(Texture2D original)
    {
        // Check the frame's Z rotation to determine which texture rotation to apply
        float zRotation = transform.eulerAngles.z;
        
        // Normalize the rotation to handle values like 270 (which is -90)
        if (zRotation > 180f) zRotation -= 360f;
        
        Debug.Log($"Frame {gameObject.name} has Z rotation: {zRotation}");
        
        if (Mathf.Approximately(zRotation, -90f)) // Left frame
        {
            // Left frame needs clockwise rotation
            return RotateTexture90Clockwise(original);
        }
        else if (Mathf.Approximately(zRotation, 90f)) // Right frame  
        {
            // Right frame needs counter-clockwise rotation
            return RotateTexture90CounterClockwise(original);
        }
        else
        {
            // No rotation or unknown rotation, use clockwise as default
            Debug.LogWarning($"Unknown frame rotation {zRotation}, using default clockwise rotation");
            return RotateTexture90Clockwise(original);
        }
    }

    Texture2D RotateTexture90CounterClockwise(Texture2D original)
    {
        int w = original.width;
        int h = original.height;
        Texture2D rotated = new Texture2D(h, w, original.format, false);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                rotated.SetPixel(h - y - 1, x, original.GetPixel(x, y));
            }
        }
        rotated.Apply();
        return rotated;
    }
}
