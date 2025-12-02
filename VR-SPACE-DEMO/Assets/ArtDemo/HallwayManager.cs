using UnityEngine;
using System.IO;

public class HallwayManager : MonoBehaviour
{
    public Transform player;
    public GameObject hallwayPrefab;
    public float segmentLength = 10f;

    private int currentIndex = 0;
    private int lastSpawnedIndex = -1;

    // Painting system
    private string[] paintingPaths;

    void Start()
    {
        string folder = Application.persistentDataPath + "/Paintings/";
        if (Directory.Exists(folder))
            paintingPaths = Directory.GetFiles(folder, "*.png");
        else
            paintingPaths = new string[0];

        SpawnSegment(0);
        SpawnSegment(1);
        SpawnSegment(2);
        lastSpawnedIndex = 2;
    }

    void Update()
    {
        float z = player.position.z;
        int newIndex = Mathf.FloorToInt(z / segmentLength);
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            OnEnteredNewSegment();
        }
    }

    void OnEnteredNewSegment()
    {
        int nextIndex = lastSpawnedIndex + 1;
        if (nextIndex < paintingPaths.Length)
        {
            SpawnSegment(nextIndex);
            lastSpawnedIndex = nextIndex;
        }
        else
        {
            if (nextIndex == paintingPaths.Length)
            {
                SpawnEndCap(nextIndex);
                lastSpawnedIndex++;
            }
        }

        foreach (Transform child in transform)
        {
            int idx = Mathf.RoundToInt(child.position.z / segmentLength);
            if (idx < currentIndex - 1)
                Destroy(child.gameObject);
        }
    }

    void SpawnSegment(int index)
    {
        Vector3 pos = new Vector3(0, 0, index * segmentLength);
        GameObject seg = Instantiate(hallwayPrefab, pos, Quaternion.identity, transform);

        PaintingFrame frame = seg.GetComponentInChildren<PaintingFrame>();
        if (frame && index < paintingPaths.Length)
            frame.LoadPainting(paintingPaths[index]);

        seg.name = $"Segment_{index}";
    }

    void SpawnEndCap(int index)
    {
        Vector3 pos = new Vector3(0, 0, index * segmentLength);
        GameObject seg = Instantiate(hallwayPrefab, pos, Quaternion.identity, transform);

        PaintingFrame frame = seg.GetComponentInChildren<PaintingFrame>();
        if (frame)
            frame.LoadBlankOrEndMessage();

        seg.name = "EndSegment";
    }
}
