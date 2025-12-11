using UnityEngine;
using System.IO;

public class PaintingFrame : MonoBehaviour
{
    public Renderer canvasRenderer;
    [Header("Price Display")]
    public Transform priceDisplayAnchor;

    public void LoadPainting(string path)
    {
        if (!File.Exists(path)) return;

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        Texture2D rotatedTex = GetCorrectlyRotatedTexture(tex);
        
        canvasRenderer.material.mainTexture = rotatedTex;
        DestroyImmediate(tex);
        LoadAndDisplayPrice(path);
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

    //jotkut on väärinpäin, jotkut ei
    Texture2D GetCorrectlyRotatedTexture(Texture2D original)
    {
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f;
        if (Mathf.Approximately(zRotation, -90f))
        {
            return RotateTexture90Clockwise(original);
        }
        else if (Mathf.Approximately(zRotation, 90f))
        {
            return RotateTexture90CounterClockwise(original);
        }
        else
        {
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
    
    void LoadAndDisplayPrice(string paintingPath)
    {
        string metadataPath = paintingPath.Replace(".png", "_metadata.txt");
        
        if (File.Exists(metadataPath))
        {
            string priceText = File.ReadAllText(metadataPath);
            if (float.TryParse(priceText, out float price))
            {
                CreatePriceDisplay(price);
            }
        }
    }
    
    void CreatePriceDisplay(float price)
    {
        GameObject priceObj = new GameObject("PriceDisplay");
        
        // Use anchor if provided, otherwise use frame's transform
        if (priceDisplayAnchor != null)
        {
            priceObj.transform.SetParent(priceDisplayAnchor);
            priceObj.transform.localPosition = Vector3.zero;
            priceObj.transform.localRotation = Quaternion.identity;
        }
        else
        {
            priceObj.transform.SetParent(transform);
            priceObj.transform.localPosition = new Vector3(-0.7f, 0.15f, 0f);
            priceObj.transform.localRotation = Quaternion.identity;
        }
        
        // Add TextMeshPro component
        var tmp = priceObj.AddComponent<TMPro.TextMeshPro>();
        tmp.text = $"${price:N0}";
        tmp.fontSize = 1.0f;
        tmp.alignment = TMPro.TextAlignmentOptions.Center;
        tmp.color = Color.yellow;
        var billboard = priceObj.AddComponent<Billboard>();
    }
    
    void OnDrawGizmos()
    {
        if (canvasRenderer == null) return;
        
        // Draw price text position
        Vector3 pricePos;
        if (priceDisplayAnchor != null)
        {
            pricePos = priceDisplayAnchor.position;
        }
        else
        {
            pricePos = transform.position + transform.TransformDirection(new Vector3(-0.7f, 0.15f, 0f));
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pricePos, 0.1f);
        
        // Draw orientation arrows on the painting
        Vector3 center = canvasRenderer.bounds.center;
        float size = 0.3f;
        
        // Red arrow pointing up (shows top of painting)
        Gizmos.color = Color.red;
        Vector3 upDir = transform.up;
        Gizmos.DrawRay(center, upDir * size);
        Gizmos.DrawSphere(center + upDir * size, 0.05f);
        
        // Green arrow pointing right (shows right side of painting)
        Gizmos.color = Color.green;
        Vector3 rightDir = transform.right;
        Gizmos.DrawRay(center, rightDir * size);
        
        // Blue arrow pointing forward (shows which way painting faces)
        Gizmos.color = Color.blue;
        Vector3 forwardDir = transform.forward;
        Gizmos.DrawRay(center, forwardDir * size);
        
        // Draw rotation info
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f;
        
#if UNITY_EDITOR
        UnityEditor.Handles.Label(center + Vector3.up * 0.6f, 
            $"Z Rot: {zRotation:F1}°\n{GetRotationInfo()}");
#endif
    }
    
    string GetRotationInfo()
    {
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f;
        
        if (Mathf.Approximately(zRotation, -90f))
            return "→ Clockwise rotation";
        else if (Mathf.Approximately(zRotation, 90f))
            return "→ Counter-clockwise rotation";
        else
            return "→ Default (Clockwise)";
    }
}


public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
