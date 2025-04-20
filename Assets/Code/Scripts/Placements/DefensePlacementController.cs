using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DefensePlacementController : MonoBehaviour
{
    private enum PlacementMethod
    {
        SideOfRoad,
        OnRoad
    };

    [Header("Defense Information")]
    [SerializeField] private PlacementMethod placementMethod;


    [Header("Sounds")]
    [SerializeField] private AudioClip placementSoundEffect;

    [Header("Unity Events")]
    public UnityEvent onPlacementSuccess;
    public UnityEvent onPlacementFail;

    public bool Placed => placed;

    private Camera mainCamera;
    private Outline outline;
    private DefenseDataObject defenseData;
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
        StartPlacement();
    }

    // Start the placement on the map following the mouse.
    void StartPlacement()
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
                // Get hit tag.
                string tag = hit.collider.gameObject.tag;

                // If placementMethod is OnRoad and tag is RoadPlacement, then snap to road.
                if (placementMethod == PlacementMethod.OnRoad)
                {
                    if (tag == "RoadPlacement")
                    {
                        SnapToRoad(hit);
                        SetValidPlacement();
                    }
                    else
                    {
                        FreePlace(hit);
                        SetInvalidPlacement();
                    }
                }

                // If placementMethod is OnRoad and tag is SideOfRoad, then allow free placement on the side of road.
                if (placementMethod == PlacementMethod.SideOfRoad)
                {
                    if (tag == "SideOfRoadPlacement")
                    {
                        SetValidPlacement();
                    }
                    else
                    {
                        SetInvalidPlacement();
                    }

                    FreePlace(hit);
                }

                // Check if mouse clicked and mouse is not over UI.
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    // Place defense.
                    PlaceDefense();
                }
            }
        }
    }

    // Load data to be used in the placement.
    public void LoadData(DefenseDataObject data)
    {
        // Set data.
        defenseData = data;
        dataLoaded = true;
    }

    // Place the defense, disable this script, and disable outline script.
    void PlaceDefense()
    {
        // Check if valid placement is true.
        if (validPlacement)
        {
            // Get round manager.
            RoundManager roundManager = RoundManager.instance;

            // Check if defense can be purchased
            if (roundManager.Coins >= defenseData.CoinCost)
            {
                // Set isPlaced to true, and disable outline script, send event to reset the defense panel.
                placed = true;
                outline.enabled = false;

                // Subtract coin amount from round manager.
                roundManager.SubtractCoins(defenseData.CoinCost);

                // Disable this script.
                enabled = false;

                // Play placement sound effect.
                SoundManager.instance.PlaySoundEffect(placementSoundEffect, transform, .5f);

                // Call onPlacementSuccess.
                onPlacementSuccess.Invoke();
            }
            else
            {
                // Call onPlacementFail.
                onPlacementFail.Invoke();
            }
        }
    }

    // Allow defense to be dragged over any part of the map.
    void FreePlace(RaycastHit hit)
    {
        Vector3 targetPosition = hit.point;
        float hitBottom = hit.collider.bounds.min.y;
        float objectSize = transform.localScale.y;
        targetPosition.y = Mathf.Max(0, hitBottom + (objectSize / 2));
        transform.position = targetPosition;
        transform.rotation = Quaternion.identity;
    }

    // Snap defense to roads.
    void SnapToRoad(RaycastHit hit)
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
    void SetValidPlacement()
    {
        // Set the placement as valid and set outline color to green.
        validPlacement = true;
        outline.OutlineColor = Color.green;
    }

    // Set placement to invalid and change outline to red.
    void SetInvalidPlacement()
    {
        // Set the placement as invalid and set outline color to red.
        validPlacement = false;
        outline.OutlineColor = Color.red;
    }
}
