
using UnityEngine;

public class BoobingAnimation : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private float _magnitude;
    [SerializeField] private Vector3 _boobindDirection;
    private Pickup pickup;
    private Vector3 startPosition;
    void Start()
    {
        pickup = GetComponent<Pickup>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (pickup && !pickup.HasBeenCollected)
            transform.position = startPosition + _boobindDirection * Mathf.Sin(_frequency * Time.time) * _magnitude;
    }
}
