using UnityEngine;
using UnityEngine.UI;

public class PlayerTrashInteract : MonoBehaviour
{
    [Header("[== RAYCAST SETTINGS ==]")]
    [SerializeField] private bool raycastDebug = false;
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private LayerMask layersToHit;

    [Header("[== REFERENCES ==]")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Inventory playerInventory;

    [Header("[== CURSOR ==]")]
    [SerializeField] private Image cursorUI;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite interactSprite;

    private Camera cam;
    private RaycastHit hit;
    private Ray ray;

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.InteractEvent += HandleInteract;
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.InteractEvent -= HandleInteract;
        }
    }

    // using new input system
    private void HandleInteract()
    {
        GameObject hitObj = castRay();
        if (hitObj != null)
        {
            HitObject(hitObj);
        }
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        cursorUI.sprite = (
            castRay() != null
            ? interactSprite
            : normalSprite
        );
    }

    GameObject castRay()
    {
        Transform camTrans = cam.transform;
        ray = new Ray(camTrans.position, camTrans.forward);
        if (Physics.Raycast(ray, out hit, maxDistance, layersToHit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (raycastDebug)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                Debug.Log(hitObj.name + " was hit from a distance of " + Vector3.Distance(camTrans.position, hitObj.transform.position));
            }
            return hitObj;
        }
        else if (raycastDebug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        }
        return null;
    }

    private void HitObject(GameObject obj)
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("No inventory component!");
            Debug.Log("testing...");
            Destroy(obj); //like old system
            return;
        }

        TrashItem trashItem = obj.GetComponent<TrashItem>();
        if (trashItem == null)
        {
            Debug.LogWarning(obj.name + "has no TrashItem component — cannot collect.");
            return;
        }

        if (playerInventory.Add(trashItem.Data))
        {
            Destroy(obj);
        }
        else
        {
            Debug.Log("Inventory full — could not collect.");
        }
    }
}