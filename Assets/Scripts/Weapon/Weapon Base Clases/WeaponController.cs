using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject _weaponStats;
    protected PlayerMovement playerMovementScript;
    private float currentCd;
    [HideInInspector] public float _speed => _weaponStats.speed;

    virtual protected void Start()
    {
        playerMovementScript = FindFirstObjectByType<PlayerMovement>();
        currentCd = _weaponStats.cdDuration;
    }

    virtual protected void Update()
    {
        currentCd -= Time.deltaTime;
        if (currentCd <= 0f)
            Attack();
    }
    
    virtual protected void Attack()
    {
        currentCd = _weaponStats.cdDuration;
    }
}
