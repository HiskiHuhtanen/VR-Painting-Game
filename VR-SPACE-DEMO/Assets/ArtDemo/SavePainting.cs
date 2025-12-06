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
        Texture2D rotated = Rotate(tex);

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
