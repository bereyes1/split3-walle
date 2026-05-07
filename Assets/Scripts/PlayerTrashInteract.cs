using System;
using UnityEngine;

public class PlayerTrashInteract : MonoBehaviour
{
    [Header("[== RAYCAST SETTINGS ==]")]
    [SerializeField] private bool raycastDebug = false;
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private InputReader inputReader;

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

    //using new input system
    private void HandleInteract()
    {
        castRay();
    }

    void Start()
    {
        cam = Camera.main;
    }

    // called when m1 button is down
    // uses old unity input system
    void OnMouseDown()
    {
        castRay();
    }

    void castRay()
    {
        Transform camTrans = cam.transform;
        ray = new Ray(camTrans.position, camTrans.forward);
        if (Physics.Raycast(ray, out hit, maxDistance, layersToHit))
        {
            GameObject hitObj = hit.collider.gameObject;
            HitObject(hitObj);

            if (raycastDebug)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                Debug.Log(hitObj.name + " was hit from a distance of " + Vector3.Distance(camTrans.position, hitObj.transform.position));
            }
        }
        else if (raycastDebug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        }
    }

    void HitObject(GameObject obj)
    {
        Destroy(obj);

        // TODO add logic after removing from gameworld
        // ie adding to trash counter, storing it into inv?
        // idk, still up in the air what we do with this!
    }
}