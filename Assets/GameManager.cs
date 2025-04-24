using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // UI-����� ��� ����������� ��������� � ������/���������
    public TextMeshProUGUI endGameText;

    // ������������ ����������� ������
    public float cameraMoveDuration = 2f;
    // �����, � ������� ������ ������������� ������ (��������, ����� ������)
    public Vector3 cameraEndPosition = Vector3.zero;
    // ���� ��������� ����
    public bool isGameOver = false;

    // ������ �� �������� ������
    private Camera mainCamera;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        if (endGameText != null)
            endGameText.text = "";
    }

    // �����, ���������� ����� ���� �������������.
    // �������� playerWon ����������, ������� �� ����� (true) ��� �������� (false).
    public void EndGame(bool playerWon)
    {
        if (isGameOver)
            return;

        isGameOver = true;

        // ���������� ����� ������ � ��������, ��������� �������-�������� (�� �������� � ���� �������)
        // ����� ����������� ������� �������� ��������� ���� ���������, ����� ��� ���������� ������.

        // ������� ��� ������������ �����
        UnitBase[] allUnits = FindObjectsOfType<UnitBase>();
        foreach (UnitBase unit in allUnits)
        {
            Destroy(unit.gameObject);
        }

        // ��������� ������������������ ��������� ����:
        // ������� ������ ���������� ������, ����� ������� ��������� � �������� ������.
        StartCoroutine(EndGameSequence(playerWon));
    }

    IEnumerator EndGameSequence(bool playerWon)
    {
        // ������� ����������� ������ �� ������� ������� � �������� (��������, ������)
        Vector3 startPos = mainCamera.transform.position;
        float elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(startPos, cameraEndPosition, elapsed / cameraMoveDuration);
            yield return null;
        }
        mainCamera.transform.position = cameraEndPosition;

        // ����� ���������� �������� ������ ������� ���������
        string message = playerWon ? "Victory" : "Defeat";
        yield return StartCoroutine(TypewriterEffect(message));
    }

    // ������ "������������" ������, ������� ���������� �� ������
    IEnumerator TypewriterEffect(string message)
    {
        endGameText.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            endGameText.text += message[i];
            yield return new WaitForSeconds(0.1f); // �������� ����� ���������� ������ �����
        }
    }
}
