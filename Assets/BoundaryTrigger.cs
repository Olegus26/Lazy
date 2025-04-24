using UnityEngine;
using TMPro;

public class BoundaryTrigger : MonoBehaviour
{
    // �������� � ����������: true ��� ������ ���� (��������, ���� ������), false ��� ������� ���� (���� �����)
    public bool isLeftBoundary;

    // ���������� ������ ��� ����, � ������� �������� ���� �������
    public int lives = 3;

    // ������ �� UI-�����, ������������ ���������� �����
    public TextMeshProUGUI livesText;

    private void Start()
    {
        UpdateLivesUI();
    }

    // ��������� ����� � �������
    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = lives.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������������, ��� �� ������ ���� ������ UnitBase � ������� ���������� isEnemy
        UnitBase unit = collision.GetComponent<UnitBase>();
        if (unit != null)
        {
            // ���� ��� ����� ������� (���� ������), �� �������� �����, ����� � ��� �������� ���� ����� (isEnemy == true)
            if (isLeftBoundary && unit.isEnemy)
            {
                lives--;
                UpdateLivesUI();
                Destroy(unit.gameObject);
                CheckGameOver();
            }
            // ���� ��� ������ ������� (���� �����), �� �������� �����, ����� � ��� �������� ���� ������ (isEnemy == false)
            else if (!isLeftBoundary && !unit.isEnemy)
            {
                lives--;
                UpdateLivesUI();
                Destroy(unit.gameObject);
                CheckGameOver();
            }
        }
    }

    // �������� ������� ��������� ����
    void CheckGameOver()
    {
        if (lives <= 0)
        {
            GameManager.Instance.EndGame(!isLeftBoundary);
        }
    }
}
