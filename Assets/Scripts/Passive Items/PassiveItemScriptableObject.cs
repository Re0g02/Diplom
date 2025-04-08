using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/PassiveItem")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField] private float _itemMultipler;
    [SerializeField] private int _itemLevel;
    [SerializeField] private GameObject _itemNextLevelPrefab;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDescription;

    public float multipler { get => _itemMultipler; private set => _itemMultipler = value; }
    public int level { get => _itemLevel; private set => _itemLevel = value; }
    public GameObject nextLevelPrefab { get => _itemNextLevelPrefab; private set => _itemNextLevelPrefab = value; }
    public Sprite icon { get => _itemIcon; private set => _itemIcon = value; }
    public string itemName { get => _itemName; private set => _itemName = value; }
    public string description { get => _itemDescription; private set => _itemDescription = value; }
}
