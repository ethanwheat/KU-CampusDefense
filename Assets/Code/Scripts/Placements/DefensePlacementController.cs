using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DefensePlacementController : MonoBehaviour
{
    private enum PlacementMethod
    {
        SideOfRoad,
        OnRoad
    };

    [Header("Defense Information")]
    [SerializeField] private PlacementMethod placementMethod;

    [Header("Unity Events")]
    public UnityEvent onPlacementSuccess;
    public UnityEvent onPlacementFail;

    private Camera mainCamera;
    private Outline outline;
    private DefenseData defenseData;
    private RoundManager roundManager;
    private MessagePopupPanelController messagePopupPanelController;
    private bool dataLoaded = false;
    private bool placed;
    private bool validPlacement;

    void Start()
    {
        // Set camera.
        mainCamera = Camera.main;

        // Set outline
        outline = gameObject.GetComponent<Outline>();
    }

    void Update()
    {
        startPlacement();
    }

    // Start the placement on the map following the mouse.
    void startPlacement()
    {
        if (dataLoaded)
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

    // Return true if object is placed else false.
    public bool isPlaced()
    {
        return placed;
    }

    // Load data to be used in the placement.
    public void loadData(DefenseData data, RoundManager manager, MessagePopupPanelController controller)
    {
        // Set data.
        defenseData = data;
        roundManager = manager;
        messagePopupPanelController = controller;
        dataLoaded = true;
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

                // Subtract coin amount from round manager.
                roundManager.subtractCoins(defenseData.getCoinCost());

                // Set the parent of the new defense.
                transform.parent = getRootGameObject("Defenses").transform;

                // Delete placement gameObject.
                GameObject placementParent = getRootGameObject("Placement");
                Destroy(placementParent);

                // Disable this script.
                this.enabled = false;

                // Call onPlacementSuccess.
                onPlacementSuccess.Invoke();
            }
            else
            {
                // Show error popup panel and call onPlacementFail.
                messagePopupPanelController.showPanel("Insufficient Coins", "You do not have enough coins to buy a " + defenseData.getName() + "!");
                onPlacementFail.Invoke();
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

    // Set placement to valid and change outline to green.
    void setValidPlacement()
    {
        // Set the placement as valid and set outline color to green.
        validPlacement = true;
        outline.OutlineColor = Color.green;
    }

    // Set placement to invalid and change outline to red.
    void setInvalidPlacement()
    {
        // Set the placement as invalid and set outline color to red.
        validPlacement = false;
        outline.OutlineColor = Color.red;
    }

    // Get root game object
    GameObject getRootGameObject(string name)
    {
        GameObject gameObject = null;

        foreach (GameObject currentGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (currentGameObject.name == name)
            {
                gameObject = currentGameObject;
                break;
            }
        }

        if (!gameObject)
        {
            gameObject = new GameObject(name);
        }

        return gameObject;
    }
}
