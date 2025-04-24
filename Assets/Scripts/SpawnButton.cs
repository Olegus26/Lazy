using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    public UnitSpawner spawner; // Ссылка на спавнер
    public int unitIndex; // Какой юнит спавнить
    public int unitCost = 5; // Цена юнита
    private Button button; // Кнопка

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SpawnUnit);
        UpdateButtonState();
        InvokeRepeating(nameof(UpdateButtonState), 0.5f, 0.5f); // Проверяем раз в полсекунды
    }

    void SpawnUnit()
    {
        if (GameManager.Instance.isGameOver)
            return;

        if (CurrencyManager.Instance.SpendCoins(unitCost))
        {
            spawner.SpawnUnit(unitIndex);
            UpdateButtonState();
        }
    }

    void UpdateButtonState()
    {
        button.interactable = CurrencyManager.Instance.coins >= unitCost;
    }
}
