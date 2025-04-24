using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    public GameObject[] unitPrefabs; // ������� ���� ������ (�������:
                                     // 0: ������, 1: ������, 2: ���������,
                                     // 3: ��������, 4: ������� ������)
    public Transform spawnPoint;      // ����� ������ ������
    public EnemyCurrencyManager currencyManager; // ������� ����� ��� �����
    public float spawnInterval = 5f;  // �������� ������� ������ (� ��������)

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
        // ������� �������� ������ ��������� (��������� �� ���������) ������
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
            return; // ������ �� ������, ���� ��� ������� ��� ������ �� ������ �����

        // �������� ������ � ���� ���������� (��������� �������� ���� ������ ������)
        int totalPlayerHealth = GetPlayerHealth();
        // �������� ���������� ������ �� ���� (� ������ ������ ������� ����� � isEnemy == true)
        int enemyCount = FindEnemiesOnField();

        // ��� ������� ���������� ����� ������ ������� ���, ������� ����� �������������� �� ��������.
        // ��� ���� ���, ��� ���� ���� ��� ������.
        Dictionary<int, float> unitWeights = new Dictionary<int, float>();
        foreach (int unitIndex in affordableUnits)
        {
            float weight = 1f; // ������� ���
            switch (unitIndex)
            {
                case 0: // ������
                    weight = 1f;
                    break;
                case 1: // ������
                    // ����� �������� ��� ������� ���������� ������
                    weight = 1.2f + 0.1f * enemyCount;
                    break;
                case 2: // ���������
                    // ��������� ���������� ��� ����������� ����������
                    weight = 1.5f + 0.1f * enemyCount;
                    break;
                case 3: // ��������
                    weight = 1f; // ������� ���
                    break;
                case 4: // ������� ������
                    // ���� � ������ ������� �����, �� ������� ���������� �������� ������
                    weight = 2.0f + (totalPlayerHealth > 300 ? 1.0f : 0f);
                    break;
                default:
                    weight = 1f;
                    break;
            }
            unitWeights[unitIndex] = weight;
        }

        // ��������� ����� ����� �����
        float totalWeight = 0f;
        foreach (var kvp in unitWeights)
        {
            totalWeight += kvp.Value;
        }

        // ��������� ������� �������� ���� ����� ���������, � ������ �����
        float randomValue = Random.Range(0f, totalWeight);
        int chosenUnitIndex = affordableUnits[0]; // �������� �� ���������
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

        // ������� ��������� ������ ���������� ����� � ������� ���������� �����
        if (currencyManager.SpendCoins(GetUnitCost(chosenUnitIndex)))
        {
            SpawnUnit(chosenUnitIndex);
        }
    }

    void SpawnUnit(int unitIndex)
    {
        // ������� ����� � ��������� ����� ������
        GameObject spawnedUnit = Instantiate(unitPrefabs[unitIndex], spawnPoint.position, Quaternion.identity);

        // �������������, ��� ���� ���� ����������� �����
        UnitBase unitScript = spawnedUnit.GetComponent<UnitBase>();
        if (unitScript != null)
        {
            unitScript.isEnemy = true;
        }
    }

    int GetUnitCost(int unitIndex)
    {
        // ��������� ������ (� �������)
        switch (unitIndex)
        {
            case 0: return 10; // ������
            case 1: return 15; // ������
            case 2: return 25; // ���������
            case 3: return 12; // ��������
            case 4: return 30; // ������� ������
            default: return 10;
        }
    }

    int FindEnemiesOnField()
    {
        // ������������ ���������� ��������� ������ (� ������� isEnemy == true)
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
        // ��������� �������� ���� ������ ������ (isEnemy == false)
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
