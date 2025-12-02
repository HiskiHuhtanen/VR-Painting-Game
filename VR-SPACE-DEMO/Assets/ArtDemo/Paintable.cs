using UnityEngine;
using Photon.Pun;
using System.IO;
public class Paintable : MonoBehaviour
{
    public int textureSize;
    private Texture2D texture;
    private Renderer renderer;
    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        renderer = GetComponent<Renderer>();
        renderer.material = new Material(renderer.material); //pitäis korjata chrashit, aiemmin sii vika että muutettii jotain ilman lupia
        texture = new Texture2D(textureSize, textureSize);
        ClearLocalTexture();
        renderer.material.mainTexture = texture;
    }

    public void PaintPhoton(Vector2 uv, Color color, float size)
    {
        if (!pv) return;
        pv.RPC("Paint", RpcTarget.All, uv, new Vector3(color.r, color.g, color.b), size);
    }


    //https://docs.unity3d.com/ScriptReference/RaycastHit.html
    //raycast antaa uv texture cordin johon piirretään
    [PunRPC]
    public void Paint(Vector2 uv, Vector3 colorVec, float size)
    {
        Color color = new Color(colorVec.x, colorVec.y, colorVec.z);
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

    public void SavePNG(string filePath)
    {
        byte[] png = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, png);
        Debug.Log("Saved Painting: " + filePath);
    }

    public void ClearLocalTexture()
    {
        Color clear = Color.white;
        for (int x = 0; x < textureSize; x++)
            for (int y = 0; y < textureSize; y++)
                texture.SetPixel(x, y, clear);

        texture.Apply();
    }

    //ei voi käyttää private juttuja
    public Texture2D GetTextureCopy()
    {
        if (texture == null) return null;
        Texture2D copy = new Texture2D(texture.width, texture.height, texture.format, false);
        copy.SetPixels(texture.GetPixels());
        copy.Apply();
        return copy;
    }
}
