using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlacementPreviewController : MonoBehaviour
{
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private GameObject trashBlockPrefab;
    [SerializeField] private GameObject previewBlockPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private float placementDistance = 2f;
    [SerializeField] private float raycastOriginHeight = 10f;
    [SerializeField] private LayerMask placementLayers;
    [SerializeField] private float previewYOffset = 0.5f;

    public event Action OnBlockPlaced;

    private GameObject previewBlock;
    private bool previewMode;
    private bool hasValidPlacement;
    private Vector3 currentPlacementPosition;
    private Quaternion currentPlacementRotation;

    private void Start()
    {
        if (previewBlockPrefab != null)
        {
            previewBlock = Instantiate(previewBlockPrefab);
            previewBlock.SetActive(false);

            Collider[] colliders = previewBlock.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            Rigidbody[] rigidbodies = previewBlock.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            SetLayerRecursively(previewBlock, LayerMask.NameToLayer("Ignore Raycast"));
        }
        else
        {
            Debug.LogWarning("PlacementPreviewController: Preview Block Prefab is missing.");
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            previewMode = !previewMode;
            Debug.Log("Preview mode: " + previewMode);
        }

        if (!previewMode || craftingSystem == null || craftingSystem.dictCount(craftingSystem.currentBlock()) <= 0)
        {
            HidePreview();
            return;
        }

        UpdatePreview();

        if (hasValidPlacement && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceBlock();
        }
    }

    private void UpdatePreview()
    {
        if (previewBlock == null || player == null)
        {
            hasValidPlacement = false;
            return;
        }

        Vector3 placementPoint = player.position + player.forward * placementDistance;
        Vector3 rayOrigin = placementPoint + Vector3.up * raycastOriginHeight;

        currentPlacementRotation = player.rotation;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastOriginHeight + 5f, placementLayers))
        {
            currentPlacementPosition = hit.point + Vector3.up * previewYOffset;

            previewBlock.transform.position = currentPlacementPosition;
            previewBlock.transform.rotation = currentPlacementRotation;
            previewBlock.SetActive(true);

            hasValidPlacement = true;
        }
        else
        {
            HidePreview();
        }
    }

    private void PlaceBlock()
    {
        if (craftingSystem == null || craftingSystem.dictCount(craftingSystem.currentBlock()) <= 0)
        {
            Debug.Log("No blocks available!");
            return;
        }

        if (!hasValidPlacement)
        {
            Debug.Log("No valid placement location!");
            return;
        }

        craftingSystem.UseBlock();

        GameObject obj = Instantiate(trashBlockPrefab, currentPlacementPosition, currentPlacementRotation);

        OnBlockPlaced?.Invoke();

        if (craftingSystem.dictCount(craftingSystem.currentBlock()) <= 0)
        {
            previewMode = false;
            HidePreview();
        }
    }

    private void HidePreview()
    {
        hasValidPlacement = false;

        if (previewBlock != null)
        {
            previewBlock.SetActive(false);
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (layer == -1) return;

        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}