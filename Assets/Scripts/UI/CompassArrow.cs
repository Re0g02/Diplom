using UnityEngine;

public class CompassArrow : MonoBehaviour
{
    private Transform target;
    public float rotationSpeed = 45f;
    private Transform playerTransform;

    void Start()
    {

    }

    void Update()
    {
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        if (playerTransform == null || target == null)
        {
            playerTransform = FindFirstObjectByType<PlayerStats>().gameObject.GetComponent<Transform>();
            target = FindFirstObjectByType<Portal>().gameObject.transform;
            return;
        }
        Vector3 direction = target.position - playerTransform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 45);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
