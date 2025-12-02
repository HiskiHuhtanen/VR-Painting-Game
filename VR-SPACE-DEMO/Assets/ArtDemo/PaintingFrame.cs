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
        canvasRenderer.material.mainTexture = tex;
    }

    public void LoadBlankOrEndMessage()
    {
        // Make canvas blank or show "End"
        Texture2D blank = new Texture2D(2, 2);
        blank.SetPixel(0, 0, Color.black);
        blank.SetPixel(1, 1, Color.black);
        blank.Apply();

        canvasRenderer.material.mainTexture = blank;
    }
}
