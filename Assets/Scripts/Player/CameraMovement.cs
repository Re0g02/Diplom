using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        transform.position = _target.position + offset;
    }
}
