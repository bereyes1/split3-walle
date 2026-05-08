using UnityEngine;
using System;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject trashBlockPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float placementDistance = 2f;
    [SerializeField] private float raycastOriginHeight = 10f;
    [SerializeField] private LayerMask placementLayers;

    public event Action OnBlockPlaced;

    private void OnEnable()
    {
        inputReader.PlaceEvent += PlaceBlock;
    }

    private void OnDisable()
    {
        inputReader.PlaceEvent -= PlaceBlock;
    }

    public void PlaceBlock()
    {
        if (craftingSystem.TrashBlockCount <= 0)
        {
            Debug.Log("No blocks available!");
            return;
        }

        Vector3 placementPoint = player.position + player.forward * placementDistance;
        Vector3 rayOrigin = placementPoint + Vector3.up * raycastOriginHeight;

        craftingSystem.UseBlock();

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastOriginHeight + 5f, placementLayers))
        {
            GameObject obj = Instantiate(trashBlockPrefab, hit.point, player.rotation);

            Renderer renderer = obj.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                float bottomOffset = obj.transform.position.y - renderer.bounds.min.y;
                obj.transform.position += Vector3.up * bottomOffset;
            }
        }
        else
        {
            Instantiate(trashBlockPrefab, placementPoint, player.rotation);
        }

        OnBlockPlaced?.Invoke();
    }
}
