
using UnityEngine;

public class BoobingAnimation : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private float _magnitude;
    [SerializeField] private Vector3 _boobindDirection;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + _boobindDirection * Mathf.Sin(_frequency * Time.time) * _magnitude;
    }
}
