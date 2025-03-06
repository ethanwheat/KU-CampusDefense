using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DefencePlacementController : MonoBehaviour
{
    public enum PlacementMethod
    {
        SideOfRoad,
        OnRoad
    };
    public bool isPlaced;
    public UnityEvent onDefensePlace;

    [SerializeField] private PlacementMethod placementMethod;

    private Camera mainCamera;
    private Outline outline;
    private bool validPlacement = false;

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
                }
                else
                {
                    freePlace(hit);
                    setInvalidPlacement();
                }
            }

            // If placementMethod is OnRoad and tag is SideOfRoad, then allow free placement on the side of road.
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

            // Check if placed.
            if (Input.GetMouseButtonDown(0))
            {
                // Place defense.
                placeDefense();
            }
        }
    }

    // Allow defense to be dragged over any part of the map.
    void freePlace(RaycastHit hit)
    {
        Vector3 targetPosition = hit.point;
        float hitBottom = hit.collider.bounds.min.y;
        float objectSize = transform.localScale.y;
        targetPosition.y = Mathf.Max(0, hitBottom + (objectSize / 2));
        transform.position = targetPosition;
        transform.rotation = Quaternion.identity;
    }

    // Snap defense to roads.
    void snapToRoad(RaycastHit hit)
    {
        Vector3 objectPosition = hit.transform.position;
        Quaternion objectRotation = hit.transform.rotation;
        Vector3 objectDirection = hit.transform.rotation * Vector3.right;

        Vector3 movementDirection = hit.point - objectPosition;
        float distanceAlongDirection = Vector3.Dot(movementDirection, objectDirection);

        Vector3 targetPosition = objectPosition + objectDirection * distanceAlongDirection;
        float hitBottom = hit.collider.bounds.min.y;
        float objectSize = transform.localScale.y;
        targetPosition.y = Mathf.Max(0, hitBottom + (objectSize / 2));

        transform.position = targetPosition;
        transform.rotation = objectRotation;
    }

    // Set the placement as valid.
    void setValidPlacement()
    {
        validPlacement = true;
        outline.OutlineColor = Color.green;
    }

    // Set the placement as invalid.
    void setInvalidPlacement()
    {
        validPlacement = false;
        outline.OutlineColor = Color.red;
    }

    // Cancel the placement
    public void cancelPlacement()
    {
        Destroy(gameObject);
    }

    // Place the defense, disable this script, and disable outline script.
    void placeDefense()
    {
        // Check if valid placement is true.
        if (validPlacement)
        {
            // Send event to reset the defense panel.
            onDefensePlace.Invoke();

            // Set isPlaced to true.
            isPlaced = true;

            // Disable this script.
            this.enabled = false;

            // Disable outline script.
            outline.enabled = false;

            // Set the parent of the new defense.
            GameObject parent = null;

            foreach (GameObject t in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (t.name == "Defenses")
                {
                    parent = t.gameObject;
                    break;
                }
            }

            if (!parent)
            {
                parent = new GameObject("Defenses");
            }

            Transform parentTransform = parent.transform;
            transform.parent = parentTransform;

            // Delete placement gameobject.
            GameObject placementGameObject = null;

            foreach (GameObject t in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (t.name == "Placement")
                {
                    placementGameObject = t.gameObject;
                    break;
                }
            }

            if (placementGameObject)
            {
                Destroy(placementGameObject);
            }
        }
    }
}
