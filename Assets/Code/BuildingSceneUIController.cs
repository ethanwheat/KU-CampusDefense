using UnityEngine;
using UnityEngine.UI;

public class BuildingSceneUIController : MonoBehaviour
{
    [SerializeField] private Button startRoundButton;

    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;

        startRoundButton.onClick.AddListener(startRound);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Building");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Hit");
                Debug.Log(hit.collider.gameObject);
            }
        }
    }

    void startRound()
    {
        // Start round.
    }
}
