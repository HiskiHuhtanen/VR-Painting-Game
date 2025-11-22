using UnityEngine;
using Photon.Pun;
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
}
