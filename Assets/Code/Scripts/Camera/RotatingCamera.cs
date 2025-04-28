using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 point = Vector3.zero;
    [SerializeField] private Vector3 axis = Vector3.up;

    void Update()
    {
        // Rotate the camera around the given point, maintaining its distance
        transform.RotateAround(point, axis, -speed * Time.deltaTime);
    }
}
