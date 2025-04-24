using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject[] unitPrefabs; // Список префабов юнитов
    public Transform spawnPoint; // Точка спавна
    public bool isEnemySpawner = false; // Этот спавнер врага?

    public void SpawnUnit(int unitIndex)
    {
        if (unitIndex < 0 || unitIndex >= unitPrefabs.Length) return;

        GameObject unit = Instantiate(unitPrefabs[unitIndex], spawnPoint.position, Quaternion.identity);

        // Проверяем, вражеский ли это спавнер, и передаём инфу юниту
        MeleeUnit movement = unit.GetComponent<MeleeUnit>();
        if (movement != null)
        {
            movement.isEnemy = isEnemySpawner;
        }
    }
}
