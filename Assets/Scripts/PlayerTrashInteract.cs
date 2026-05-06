using UnityEngine;

public class PlayerTrashInteract : MonoBehaviour
{
    [Header("[== RAYCAST SETTINGS ==]")]
    public float maxDistance = 25f;
    public LayerMask layersToHit;
    
    private Camera cam;
    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ray = new Ray(cam.transform.position, cam.transform.forward);

        bool wasHit = Physics.Raycast(ray, out hit, maxDistance, layersToHit);
        if (wasHit)
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            Debug.Log(hit.collider.gameObject.name + " was hit!");
        }
        else
        {
            // debug line
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        }
    }
}
