using UnityEngine;
using System;

public class PlacementSystem : MonoBehaviour
{
    [Header("[== SYSTEM REFERENCES ==]")]
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private InputReader inputReader;
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
        if (craftingSystem.CurrentBlockCount <= 0)
        {
            Debug.Log($"No blocks available! Current Selected Block: {craftingSystem.CurrentBlock}");
            return;
        }

        Vector3 placementPoint = player.position + player.forward * placementDistance;
        Vector3 rayOrigin = placementPoint + Vector3.up * raycastOriginHeight;

        craftingSystem.UseBlock();

        GameObject trashBlockPrefab = craftingSystem.CurrentBlock.prefab;
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
