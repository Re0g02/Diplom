using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField] private float _enemySpeed;
    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _enemyDamage;
    public float speed { get => _enemySpeed; private set => _enemySpeed = value;  }
    public float health { get => _enemyHealth;private set => _enemyHealth = value; }
    public float damage { get => _enemyDamage;private set => _enemyDamage = value; }
}
