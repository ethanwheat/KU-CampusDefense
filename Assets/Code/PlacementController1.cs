using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public enum PlacementMethod
    {
        SideOfRoad,
        OnRoad
    };

    public PlacementMethod placementMethod;

    private Camera mainCamera;
    private Outline outline;

    void Start()
    {
        mainCamera = Camera.main;
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Create ray from point on screen to point on world.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Get layer mask.
        int layerMask = LayerMask.GetMask("Placement");

        // Create raycast.
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            string tag = hit.collider.gameObject.tag;

            // If placementMethod is OnRoad and tag is RoadPlacement, then snap to road.
            if (placementMethod == PlacementMethod.OnRoad)
            {
                if (tag == "RoadPlacement")
                {
                    snapToRoad(hit);
                    setValidPlacement();

                    return;
                }

                freePlace(hit);
                setInvalidPlacement();

                return;
            }

            if (placementMethod == PlacementMethod.SideOfRoad)
            {
                if (tag == "SideOfRoadPlacement")
                {
                    setValidPlacement();
                }
                else
                {
                    setInvalidPlacement();
                }

                freePlace(hit);
            }
        }
    }

    void freePlace(RaycastHit hit)
    {
        transform.position = hit.point;
        transform.rotation = Quaternion.identity;
    }

    void snapToRoad(RaycastHit hit)
    {
        Vector3 objectPosition = hit.transform.position;
        Quaternion objectRotation = hit.transform.rotation;
        Vector3 objectDirection = hit.transform.rotation * Vector3.right;

        Vector3 movementDirection = hit.point - objectPosition;
        float distanceAlongDirection = Vector3.Dot(movementDirection, objectDirection);

        Vector3 targetPosition = objectPosition + objectDirection * distanceAlongDirection;
        targetPosition.y = 0;

        transform.position = targetPosition;
        transform.rotation = objectRotation;
    }

    void setValidPlacement()
    {
        outline.OutlineColor = Color.green;
    }

    void setInvalidPlacement()
    {
        outline.OutlineColor = Color.red;
    }
}
