using UnityEngine;

public class SavePainting : Interactive
{
    Paintable paintable;

    void Start()
    {
        paintable = FindObjectOfType<Paintable>();
    }

    public new void Interact()
    {
        SaveCurrentPainting();
        paintable.ClearLocalTexture();
    }

    void SaveCurrentPainting()
    {
        if (!paintable) return;

        string folder = Application.persistentDataPath + "/Paintings/";
        if (!System.IO.Directory.Exists(folder))
            System.IO.Directory.CreateDirectory(folder);
        string filename = "Painting_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullpath = folder + filename;


        Texture2D originalCopy = paintable.GetTextureCopy();
        if (originalCopy == null)
        {
            Debug.LogError("SavePainting: paintable texture was null.");
            return;
        }
        Texture2D rotatedTexture = Rotate(originalCopy);
        SavePNG(fullpath, rotatedTexture);
        Debug.Log("Painting saved to: " + fullpath);
    }

    Texture2D Rotate(Texture2D original)
    {
        int w = original.width;
        int h = original.height;
        Texture2D rotated = new Texture2D(h, w, TextureFormat.RGBA32, false);

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

    void SavePNG(string path, Texture2D tex)
    {
        byte[] png = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, png);
    }
}
