using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    public GameObject[] unitPrefabs; // Префабы всех юнитов (индексы:
                                     // 0: Мечник, 1: Лучник, 2: Кавалерия,
                                     // 3: Копейщик, 4: Элитный мечник)
    public Transform spawnPoint;      // Точка спавна юнитов
    public EnemyCurrencyManager currencyManager; // Система монет для врага
    public float spawnInterval = 5f;  // Интервал попыток спавна (в секундах)

    private float spawnTimer = 0f;

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            StrategicallySpawnUnits();
            spawnTimer = 0f;
        }
    }

    void StrategicallySpawnUnits()
    {
        // Сначала получаем список доступных (доступных по стоимости) юнитов
        List<int> affordableUnits = new List<int>();
        for (int i = 0; i < unitPrefabs.Length; i++)
        {
            int cost = GetUnitCost(i);
            if (currencyManager.coins >= cost)
            {
                affordableUnits.Add(i);
            }
        }

        if (affordableUnits.Count == 0)
            return; // Ничего не делаем, если нет средств для спавна ни одного юнита

        // Получаем данные о силе противника (суммарное здоровье всех юнитов игрока)
        int totalPlayerHealth = GetPlayerHealth();
        // Получаем количество врагов на поле (в данном случае считаем юниты с isEnemy == true)
        int enemyCount = FindEnemiesOnField();

        // Для каждого доступного юнита задаем базовый вес, который можно корректировать по ситуации.
        // Чем выше вес, тем выше шанс его спавна.
        Dictionary<int, float> unitWeights = new Dictionary<int, float>();
        foreach (int unitIndex in affordableUnits)
        {
            float weight = 1f; // базовый вес
            switch (unitIndex)
            {
                case 0: // Мечник
                    weight = 1f;
                    break;
                case 1: // Лучник
                    // Лучше подходит при большом количестве врагов
                    weight = 1.2f + 0.1f * enemyCount;
                    break;
                case 2: // Кавалерия
                    // Кавалерия эффективна при агрессивном противнике
                    weight = 1.5f + 0.1f * enemyCount;
                    break;
                case 3: // Копейщик
                    weight = 1f; // базовый вес
                    break;
                case 4: // Элитный мечник
                    // Если у игрока большая армия, то элитник становится особенно ценным
                    weight = 2.0f + (totalPlayerHealth > 300 ? 1.0f : 0f);
                    break;
                default:
                    weight = 1f;
                    break;
            }
            unitWeights[unitIndex] = weight;
        }

        // Вычисляем общую сумму весов
        float totalWeight = 0f;
        foreach (var kvp in unitWeights)
        {
            totalWeight += kvp.Value;
        }

        // Случайным образом выбираем юнит среди доступных, с учетом весов
        float randomValue = Random.Range(0f, totalWeight);
        int chosenUnitIndex = affordableUnits[0]; // значение по умолчанию
        foreach (int unitIndex in affordableUnits)
        {
            float w = unitWeights[unitIndex];
            if (randomValue < w)
            {
                chosenUnitIndex = unitIndex;
                break;
            }
            randomValue -= w;
        }

        // Пробуем потратить нужное количество монет и спавним выбранного юнита
        if (currencyManager.SpendCoins(GetUnitCost(chosenUnitIndex)))
        {
            SpawnUnit(chosenUnitIndex);
        }
    }

    void SpawnUnit(int unitIndex)
    {
        // Создаем юнита в указанной точке спавна
        GameObject spawnedUnit = Instantiate(unitPrefabs[unitIndex], spawnPoint.position, Quaternion.identity);

        // Устанавливаем, что этот юнит принадлежит врагу
        UnitBase unitScript = spawnedUnit.GetComponent<UnitBase>();
        if (unitScript != null)
        {
            unitScript.isEnemy = true;
        }
    }

    int GetUnitCost(int unitIndex)
    {
        // Стоимость юнитов (в монетах)
        switch (unitIndex)
        {
            case 0: return 10; // Мечник
            case 1: return 15; // Лучник
            case 2: return 25; // Кавалерия
            case 3: return 12; // Копейщик
            case 4: return 30; // Элитный мечник
            default: return 10;
        }
    }

    int FindEnemiesOnField()
    {
        // Подсчитываем количество вражеских юнитов (у которых isEnemy == true)
        UnitBase[] allUnits = FindObjectsOfType<UnitBase>();
        int enemyCount = 0;
        foreach (UnitBase unit in allUnits)
        {
            if (!unit.isEnemy)
            {
                enemyCount++;
            }
        }
        return enemyCount;
    }

    int GetPlayerHealth()
    {
        // Суммируем здоровье всех юнитов игрока (isEnemy == false)
        UnitBase[] allUnits = FindObjectsOfType<UnitBase>();
        int totalHealth = 0;
        foreach (UnitBase unit in allUnits)
        {
            if (!unit.isEnemy)
            {
                totalHealth += unit.health;
            }
        }
        return totalHealth;
    }
}
