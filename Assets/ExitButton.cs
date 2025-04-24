using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button button; // ������

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ExitGame);
        
    }

    public void ExitGame()
    {
        // ���� ���� �������� � ������, ��������� ����������
        Application.Quit();

        // ���� �� ���������� � ���������, ����� ������� ���������
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
