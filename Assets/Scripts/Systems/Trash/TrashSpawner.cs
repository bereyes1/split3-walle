using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
// public class TrashEntry
// {
//     public GameObject prefab;
//     public float weight = 1f;
// }

public class TrashSpawner : MonoBehaviour
{
    [Header("Trash Types")]
    [SerializeField] private List<ItemData> trashTypes;

    [Header("Poisson Settings")]
    [SerializeField] private float radius = 3f;
    [SerializeField] private Vector2 regionSize = new Vector2(100f, 100f);
    [SerializeField] private int rejectionSamples = 30;

    [Header("Placement")]
    [SerializeField] private float raycastOriginHeight = 100f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Gizmos")]
    [SerializeField] private float displayRadius = 0.5f;

    private List<Vector2> samplePoints;

    private void Start()
    {
        GenerateSamplePoints();
        SpawnTrash();
    }

    private void GenerateSamplePoints()
    {
        samplePoints = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    private void SpawnTrash()
    {
        if (trashTypes == null || trashTypes.Count == 0)
        {
            Debug.LogWarning("TrashSpawner: no trash types assigned.");
            return;
        }

        float totalWeight = 0f;
        foreach (ItemData entry in trashTypes)
        {
            totalWeight += entry.weight;
        }

        GameObject container = new GameObject("--- Trash ---");
        Vector3 origin = transform.position - new Vector3(regionSize.x / 2f, 0f, regionSize.y / 2f);

        int placed = 0;
        foreach (Vector2 point in samplePoints)
        {
            Vector3 rayOrigin = new Vector3(origin.x + point.x, transform.position.y + raycastOriginHeight, origin.z + point.y);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastOriginHeight + 200f, groundLayers)) continue;

            GameObject trashPrefab = PickWeightedRandom(totalWeight);
            if (trashPrefab == null) continue;

            GameObject obj = Instantiate(trashPrefab, hit.point, Quaternion.identity, container.transform);

            Renderer renderer = obj.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                float bottomOffset = obj.transform.position.y - renderer.bounds.min.y;
                obj.transform.position += Vector3.up * bottomOffset;
            }

            placed++;
        }

        Debug.Log($"TrashSpawner: placed {placed}/{samplePoints.Count} trash objects.");
    }

    private GameObject PickWeightedRandom(float totalWeight)
    {
        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        foreach (ItemData entry in trashTypes)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
            {
                return entry.prefab;
            }
        }
        return trashTypes[trashTypes.Count - 1].prefab;
    }

    private void OnValidate()
    {
        samplePoints = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(regionSize.x, 0f, regionSize.y));

        if (samplePoints == null) return;

        Gizmos.color = Color.cyan;
        Vector3 origin = transform.position - new Vector3(regionSize.x / 2f, 0f, regionSize.y / 2f);
        foreach (Vector2 point in samplePoints)
        {
            Vector3 worldPos = new Vector3(origin.x + point.x, transform.position.y, origin.z + point.y);
            Gizmos.DrawSphere(worldPos, displayRadius);
        }
    }
}
