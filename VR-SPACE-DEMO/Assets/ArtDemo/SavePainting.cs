using UnityEngine;

public class SavePainting : Interactive
{
    Paintable paintable;

    public AppraisalDisplay appraisalDisplay;

    void Start()
    {
        paintable = FindObjectOfType<Paintable>();
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
        Debug.Log($"Original texture: {tex.width}x{tex.height}, format: {tex.format}");
        
        // Check if original texture has any non-white pixels
        int nonWhitePixels = 0;
        Color[] pixels = tex.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] != Color.white)
                nonWhitePixels++;
        }
        Debug.Log($"Original texture has {nonWhitePixels} non-white pixels out of {pixels.Length}");
        
        Texture2D rotated = Rotate(tex);
        Debug.Log($"Rotated texture: {rotated.width}x{rotated.height}");
        
        // Check if rotated texture has any non-white pixels
        int rotatedNonWhitePixels = 0;
        Color[] rotatedPixels = rotated.GetPixels();
        for (int i = 0; i < rotatedPixels.Length; i++)
        {
            if (rotatedPixels[i] != Color.white)
                rotatedNonWhitePixels++;
        }
        Debug.Log($"Rotated texture has {rotatedNonWhitePixels} non-white pixels out of {rotatedPixels.Length}");

        System.IO.File.WriteAllBytes(fullpath, rotated.EncodeToPNG());
        Debug.Log("Painting saved to: " + fullpath);
        float value = Appraisal.Appraise(rotated);

        Debug.Log("Painting value: " + value);
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
