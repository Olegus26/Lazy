using UnityEngine;

public class EnemyCurrencyManager : MonoBehaviour
{
    public int coins = 0; // ������ ��
    public int coinsPerSecond = 1; // ���������� ����� � �������
    public float incomeRate = 1f; // �������� ����������� �����

    private void Start()
    {
        InvokeRepeating(nameof(AddCoin), 1f, incomeRate); // ��������� �����
    }

    void AddCoin()
    {
        coins += coinsPerSecond;
    }

    // ����� ��� ����� �����
    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true;
        }
        return false;
    }
}
