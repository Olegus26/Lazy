using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button button; // Кнопка

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ExitGame);
        
    }

    public void ExitGame()
    {
        // Если игра запущена в сборке, завершает приложение
        Application.Quit();

        // Если ты тестируешь в редакторе, можно вывести сообщение
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
