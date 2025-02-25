using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public enum PlacementMethod
    {
        SideOfRoad,
        OnRoad
    };

    public PlacementMethod placementMethod;

    [SerializeField] private Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        // Create ray from point on screen to point on world.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Get layer mask.
        int layerMask = LayerMask.GetMask("Placement");

        // Create raycast.
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
        {
            GameObject placement = raycastHit.collider.gameObject;
            string tag = placement.tag;

            // If placementMethod is OnRoad and tag is RoadPlacement, then snap to road.
            if (placementMethod == PlacementMethod.OnRoad && tag == "RoadPlacement")
            {
                Vector3 mousePosition = raycastHit.point;
                Vector3 targetPosition = raycastHit.transform.position;
                Quaternion targetRotation = raycastHit.transform.rotation;

                transform.position = new Vector3(mousePosition.x, 0, targetPosition.z);
                transform.rotation = targetRotation;
            }
            else
            {
                transform.position = raycastHit.point;
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
