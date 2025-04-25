using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataScriptableObject : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string playerName;
    [SerializeField] private WeaponDataScriptableObject startingWeapon;
    public Sprite Icon { get => icon; private set => icon = value; }
    public string Name { get => playerName; private set => playerName = value; }
    public WeaponDataScriptableObject StartingWeapon { get => startingWeapon; private set => startingWeapon = value; }

    [System.Serializable]
    public struct Stats
    {
        public float maxHealth;
        public float recovery;
        public float moveSpeed;
        public float might;
        public float speed;
        public float magnet;

        public Stats(float maxHealth = 1000f, float recovery = 0f, float moveSpeed = 1f, float might = 1f, float speed = 1f, float magnet = 30f)
        {
            this.maxHealth = maxHealth;
            this.recovery = recovery;
            this.moveSpeed = moveSpeed;
            this.might = might;
            this.speed = speed;
            this.magnet = magnet;
        }

        public static Stats operator +(Stats l, Stats r)
        {
            l.maxHealth += r.maxHealth;
            l.recovery += r.recovery;
            l.moveSpeed += r.moveSpeed;
            l.might += r.might;
            l.speed += r.speed;
            l.magnet += r.magnet;
            return l;
        }
    }
    [SerializeField] private Stats stats = new Stats(1000);
    public Stats playerStats { get => stats; }
}