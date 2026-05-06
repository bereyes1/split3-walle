using UnityEngine;

public class PlayerTrashInteract : MonoBehaviour
{
    [Header("[== RAYCAST SETTINGS ==]")]
    public bool raycastDebug = false;
    public float maxDistance = 25f;
    public LayerMask layersToHit;
    
    private Camera cam;
    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        castRay();
    }

    void castRay()
    {
        ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out hit, maxDistance, layersToHit))
        {
            GameObject hitObj = hit.collider.gameObject;
            HitObject(hitObj);

            if (raycastDebug)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                Debug.Log(hitObj.name + " was hit!");
            }
        }
        else if (raycastDebug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        }
    }

    void HitObject(GameObject obj)
    {
        Debug.Log(obj.name + " was destroyed!");
        Destroy(obj);

        // TODO add logic after removing from gameworld
        // ie adding to trash counter, storing it into inv?
        // idk, still up in the air what we do with this!
    }
}