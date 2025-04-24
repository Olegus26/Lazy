using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject[] unitPrefabs; // ������ �������� ������
    public Transform spawnPoint; // ����� ������
    public bool isEnemySpawner = false; // ���� ������� �����?

    public void SpawnUnit(int unitIndex)
    {
        if (unitIndex < 0 || unitIndex >= unitPrefabs.Length) return;

        GameObject unit = Instantiate(unitPrefabs[unitIndex], spawnPoint.position, Quaternion.identity);

        // ���������, ��������� �� ��� �������, � ������� ���� �����
        MeleeUnit movement = unit.GetComponent<MeleeUnit>();
        if (movement != null)
        {
            movement.isEnemy = isEnemySpawner;
        }
    }
}
