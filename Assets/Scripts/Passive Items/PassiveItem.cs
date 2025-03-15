using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats playerStats;
    [SerializeField] protected PassiveItemScriptableObject passiveItemStats;
    [HideInInspector] public PassiveItemScriptableObject stats { get => passiveItemStats; private set => passiveItemStats = value; }

    protected virtual void ApplyModifier() { }

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        ApplyModifier();
    }
}
