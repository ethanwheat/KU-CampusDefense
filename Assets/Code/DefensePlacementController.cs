using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DefencePlacementController : MonoBehaviour
{
    private enum PlacementMethod
    {
        SideOfRoad,
        OnRoad
    };

    [Header("Defense Information")]
    [SerializeField] private PlacementMethod placementMethod;

    [Header("UI Controllers")]
    [SerializeField] private MessagePopupPanelController messagePopupPanelController;

    [Header("Unity Events")]
    public UnityEvent onCancelPlacement;

    private Camera mainCamera;
    private Outline outline;
    private DefenseData defenseData;
    private RoundManager roundManager;
    private RoundSceneUIController roundSceneUIController;
    private bool placed;
    private bool validPlacement;

    void Start()
    {
        // Set camera.
        mainCamera = Camera.main;

        // Set round manager.
        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == "RoundManager")
            {
                roundManager = currentGameObject.GetComponent<RoundManager>();
            }

            if (currentGameObject.name == "RoundSceneUI")
            {
                roundSceneUIController = currentGameObject.GetComponent<RoundSceneUIController>();
            }

            if (roundManager && roundSceneUIController)
            {
                break;
            }
        }

        // Set and enable outline
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = true;
    }

    void Update()
    {
        startPlacement();
    }

    public bool isPlaced()
    {
        // Return true if object is placed else false.
        return placed;
    }

    public void setDefenseData(DefenseData data)
    {
        // Set defense data.
        defenseData = data;
    }

    void startPlacement()
    {
        if (defenseData != null)
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
    }

    // Place the defense, disable this script, and disable outline script.
    void placeDefense()
    {
        // Check if valid placement is true.
        if (validPlacement)
        {
            // Check if defense can be purchased
            if (roundManager.getCoinAmount() >= defenseData.getCoinCost())
            {
                // Set isPlaced to true, and disable outline script, send event to reset the defense panel.
                placed = true;
                outline.enabled = false;
                onCancelPlacement.Invoke();

                // Subtract coin amount from round manager.
                roundManager.subtractCoins(defenseData.getCoinCost());

                // Set the parent of the new defense.
                GameObject defenseParent = null;

                foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    if (gameObject.name == "Defenses")
                    {
                        defenseParent = currentGameObject;
                        break;
                    }
                }

                if (!defenseParent)
                {
                    defenseParent = new GameObject("Defenses");
                }

                Transform parentTransform = defenseParent.transform;
                transform.parent = parentTransform;

                // Delete placement gameObject.
                GameObject placementParent = null;

                foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    if (currentGameObject.name == "Placement")
                    {
                        placementParent = currentGameObject;
                        break;
                    }
                }

                if (placementParent)
                {
                    Destroy(placementParent);
                }

                // Disable this script.
                this.enabled = false;
            }
            else
            {
                // Show error popup panel and cancel placement.
                messagePopupPanelController.showPanel("Insufficient Coins", "You do not have enough coins to buy a " + defenseData.getName() + "!");
                onCancelPlacement.Invoke();
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

    void setValidPlacement()
    {
        // Set the placement as valid and set outline color to green.
        validPlacement = true;
        outline.OutlineColor = Color.green;
    }

    void setInvalidPlacement()
    {
        // Set the placement as invalid and set outline color to red.
        validPlacement = false;
        outline.OutlineColor = Color.red;
    }
}
