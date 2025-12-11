using UnityEngine;
using System.IO;

public class HallwayManager : MonoBehaviour
{
    public Transform player;
    public GameObject startHallwayPrefab;
    public GameObject middleHallwayPrefab;
    public GameObject endHallwayPrefab;
    public float segmentLength = 10f;

    private int currentIndex = 0;
    private int lastSpawnedIndex = -1;
    private string[] paintingPaths;
    private int totalFrames = 0;

    //spawnataan niin monta osaa kuin maalauksia
    //yritetään vähentää lagia, spawnaamalla osittain
    void Start()
    {
        string folder = Application.persistentDataPath + "/Paintings/";
        Debug.Log($"Looking for paintings in folder: {folder}");
        
        if (Directory.Exists(folder))
        {
            paintingPaths = Directory.GetFiles(folder, "*.png");
            Debug.Log($"Found {paintingPaths.Length} paintings:");
            for (int i = 0; i < paintingPaths.Length; i++)
            {
                Debug.Log($"  {i}: {paintingPaths[i]}");
            }
        }
        else
        {
            paintingPaths = new string[0];
            Debug.LogWarning($"Paintings folder does not exist: {folder}");
        }

        totalFrames = paintingPaths.Length;
        int segmentsNeeded = Mathf.CeilToInt(totalFrames / 2f);
        int initialSegments = Mathf.Max(5, Mathf.Min(segmentsNeeded + 2, 8));
        Debug.Log($"Spawning {initialSegments} initial segments");
        
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment(i);
        }
        lastSpawnedIndex = initialSegments - 1;
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
        int segmentsNeeded = Mathf.CeilToInt(totalFrames / 2f);
        for (int i = 0; i < 2; i++)
        {
            int indexToSpawn = nextIndex + i;
            
            if (indexToSpawn < segmentsNeeded)
            {
                SpawnSegment(indexToSpawn);
                lastSpawnedIndex = indexToSpawn;
            }
            else if (indexToSpawn == segmentsNeeded && lastSpawnedIndex < segmentsNeeded)
            {
                SpawnEndSegment(indexToSpawn);
                lastSpawnedIndex = indexToSpawn;
                break;
            }
        }
        foreach (Transform child in transform)
        {
            int idx = Mathf.RoundToInt(child.position.z / segmentLength);
            if (idx < currentIndex - 2)
                Destroy(child.gameObject);
        }
    }

    void SpawnSegment(int index)
    {
        Vector3 pos = new Vector3(0, 0, index * segmentLength);
        GameObject prefabToUse = GetHallwayPrefab(index);   
        GameObject seg = Instantiate(prefabToUse, pos, Quaternion.identity, transform);
        PaintingFrame[] frames = seg.GetComponentsInChildren<PaintingFrame>();

        if (frames.Length == 0)
        {
            for (int c = 0; c < seg.transform.childCount; c++)
            {
                var child = seg.transform.GetChild(c);
                var components = child.GetComponents<Component>();
                var componentNames = new string[components.Length];
                for (int comp = 0; comp < components.Length; comp++)
                {
                    componentNames[comp] = components[comp].GetType().Name;
                }
            }
        }
        
        for (int i = 0; i < frames.Length && i < 2; i++)
        {
            int paintingIndex = index * 2 + i;         
            if (paintingIndex < paintingPaths.Length)
            {
                frames[i].LoadPainting(paintingPaths[paintingIndex]);
            }
            else
            {
                frames[i].LoadBlankOrEndMessage();
            }
        }

        seg.name = $"Segment_{index}";
    }

    GameObject GetHallwayPrefab(int index)
    {
        int segmentsNeeded = Mathf.CeilToInt(totalFrames / 2f);
        
        if (index == 0)
        {
            return startHallwayPrefab != null ? startHallwayPrefab : middleHallwayPrefab;
        }
        else if (index == segmentsNeeded - 1 && segmentsNeeded > 1)
        {
            return endHallwayPrefab != null ? endHallwayPrefab : middleHallwayPrefab;
        }
        else
        {
            return middleHallwayPrefab;
        }
    }

    void SpawnEndSegment(int index)
    {
        Vector3 pos = new Vector3(0, 0, index * segmentLength);
        GameObject seg = Instantiate(endHallwayPrefab, pos, Quaternion.identity, transform);
        PaintingFrame[] frames = seg.GetComponentsInChildren<PaintingFrame>();
        foreach (var frame in frames)
        {
            if (frame != null)
                frame.LoadBlankOrEndMessage();
        }

        seg.name = "EndSegment";
    }
}
