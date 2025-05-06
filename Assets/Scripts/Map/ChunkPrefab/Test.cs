using UnityEngine;
using UnityEngine.UI; // Для работы с Slider

public class UIManager : MonoBehaviour
{
    public GameObject villagerInfoPanel;  // Панель с информацией о жителе
    public Slider strengthSlider;         // Слайдер для силы
    public Slider enduranceSlider;        // Слайдер для выносливости
    public Slider technicalSkillsSlider;  // Слайдер для технических навыков
    public GameObject highlightIndicatorPrefab; // Ссылка на HighlightIndicator (сферу)
    private GameObject currentHighlightIndicator;

    private Villager currentVillager; // Текущий выбранный житель

    // Метод для открытия панели с информацией о выбранном жителе
    public void OpenVillagerInfo(Villager villager)
    {
        currentVillager = villager;
        villagerInfoPanel.SetActive(true);  // Показываем панель с информацией о жителе
        if (currentHighlightIndicator != null) Destroy(currentHighlightIndicator.gameObject);
        currentHighlightIndicator = Instantiate(highlightIndicatorPrefab, villager.gameObject.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
        currentHighlightIndicator.transform.SetParent(villager.gameObject.transform);
        // Отображаем навыки через слайдеры
        strengthSlider.value = villager.skills.strength;
        enduranceSlider.value = villager.skills.endurance;
        technicalSkillsSlider.value = villager.skills.technicalSkills;
    }

    // Метод для скрытия HighlightIndicator
    public void CloseVillagerInfo()
    {
        villagerInfoPanel.SetActive(false); // Скрываем панель
        Destroy(currentHighlightIndicator.gameObject);
    }

    // Метод для скрытия панели при старте
    private void Start()
    {
        villagerInfoPanel.SetActive(false); // Скрываем панель с информацией при старте игры
    }

    // Обновляем позицию HighlightIndicator в каждом кадре
    private void Update()
    {

        // Проверка на клик по объектам в сцене
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Если кликнули на объект не типа Villager, скрываем панель
                if (hit.collider.GetComponent<Villager>() == null)
                {
                    CloseVillagerInfo();
                }
                else
                {
                    OpenVillagerInfo(hit.collider.GetComponent<Villager>());
                }
            }
        }
    }
}
