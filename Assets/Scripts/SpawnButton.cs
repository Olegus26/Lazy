using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    public UnitSpawner spawner; // ������ �� �������
    public int unitIndex; // ����� ���� ��������
    public int unitCost = 5; // ���� �����
    private Button button; // ������

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SpawnUnit);
        UpdateButtonState();
        InvokeRepeating(nameof(UpdateButtonState), 0.5f, 0.5f); // ��������� ��� � ����������
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
