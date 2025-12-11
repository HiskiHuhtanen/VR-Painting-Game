using UnityEngine;

public class SavePainting : Interactive
{
    Paintable paintable;

    public AppraisalDisplay appraisalDisplay;

    void Start()
    {
        paintable = FindFirstObjectByType<Paintable>();
    }

    public new void Interact()
    {
        SaveAndAppraise();
    }

    void SaveAndAppraise()
    {
        if (!paintable) return;

        string folder = Application.persistentDataPath + "/Paintings/";
        if (!System.IO.Directory.Exists(folder))
            System.IO.Directory.CreateDirectory(folder);
        string filename = "Painting_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullpath = folder + filename;
        
        Texture2D tex = paintable.GetTextureCopy();
        
        int nonWhitePixels = 0;
        Color[] pixels = tex.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] != Color.white)
                nonWhitePixels++;
        }
        
        Texture2D rotated = Rotate(tex);
        Debug.Log($"Rotated texture: {rotated.width}x{rotated.height}");
        //Melkein aina väärin päin, kääntäminen numero 9430548230
        int rotatedNonWhitePixels = 0;
        Color[] rotatedPixels = rotated.GetPixels();
        for (int i = 0; i < rotatedPixels.Length; i++)
        {
            if (rotatedPixels[i] != Color.white)
                rotatedNonWhitePixels++;
        }

        System.IO.File.WriteAllBytes(fullpath, rotated.EncodeToPNG());
        Debug.Log("Painting saved to: " + fullpath);
        float value = Appraisal.Appraise(rotated);

        //tulee vähän paljon tiedostoja mutta on se sen arvosta!
        string metadataPath = fullpath.Replace(".png", "_metadata.txt");
        System.IO.File.WriteAllText(metadataPath, value.ToString());
        if (appraisalDisplay != null)
            appraisalDisplay.ShowValue(value, () => paintable.ClearLocalTexture());
        else
            paintable.ClearLocalTexture();
    }

    Texture2D Rotate(Texture2D original)
    {
        int w = original.width;
        int h = original.height;
        Texture2D rotated = new Texture2D(h, w, TextureFormat.RGBA32, false);

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                rotated.SetPixel(h - y - 1, x, original.GetPixel(x, y));

        rotated.Apply();
        return rotated;
    }
}
