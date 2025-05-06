using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsTable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI maxHealthValue;
    [SerializeField] private TextMeshProUGUI recoveryValue;
    [SerializeField] private TextMeshProUGUI moveSpeedValue;
    [SerializeField] private TextMeshProUGUI mightValue;
    [SerializeField] private TextMeshProUGUI projectileSpeedValue;
    [SerializeField] private TextMeshProUGUI magnetValue;

    public void SetStatsTableInfo(PlayerDataScriptableObject playerData)
    {
        characterName.text = playerData.Name;
        weaponName.text = playerData.StartingWeapon.BaseStats.name;
        weaponIcon.sprite = playerData.StartingWeapon.Icon;
        maxHealthValue.text = playerData.playerStats.maxHealth.ToString();
        recoveryValue.text = playerData.playerStats.recovery.ToString();
        moveSpeedValue.text = playerData.playerStats.moveSpeed.ToString();
        mightValue.text = playerData.playerStats.might.ToString();
        projectileSpeedValue.text = playerData.StartingWeapon.BaseStats.speed.ToString();
        magnetValue.text = playerData.playerStats.magnet.ToString();
    }
}
