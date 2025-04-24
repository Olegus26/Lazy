using UnityEngine;

public class EnemyCurrencyManager : MonoBehaviour
{
    public int coins = 0; // Монеты ИИ
    public int coinsPerSecond = 1; // Количество монет в секунду
    public float incomeRate = 1f; // Скорость поступления монет

    private void Start()
    {
        InvokeRepeating(nameof(AddCoin), 1f, incomeRate); // Генерация монет
    }

    void AddCoin()
    {
        coins += coinsPerSecond;
    }

    // Метод для траты монет
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
