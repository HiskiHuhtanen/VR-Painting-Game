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
        Texture2D rotatedTex = RotateTexture90Clockwise(tex);
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
                rotated.SetPixel(h - y - 1, x, original.GetPixel(x, y));
            }
        }
        rotated.Apply();
        return rotated;
    }
}
