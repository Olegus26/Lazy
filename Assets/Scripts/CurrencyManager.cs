using UnityEngine;
using TMPro;
using System.Globalization;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int coins = 0; // ������ ��
    public int coinsPerSecond = 1; // ���������� ����� � �������
    public float incomeRate = 1f; // �������� ����������� �����
    public TextMeshProUGUI coinText;


    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        UpdateUI();
        InvokeRepeating(nameof(AddCoin), 1f, incomeRate);
    }

    void AddCoin()
    {
        if (GameManager.Instance.isGameOver)
            return;

        coins += coinsPerSecond;
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = FormatNumber(coins);
    }

    string FormatNumber(int number)
    {
        return number.ToString("N0", new CultureInfo("ru-RU")); // ������ � ������������� (������� ��������)
    }
}
