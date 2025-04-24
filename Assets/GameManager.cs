using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // UI-текст для отображения сообщения о победе/поражении
    public TextMeshProUGUI endGameText;

    // Длительность перемещения камеры
    public float cameraMoveDuration = 2f;
    // Точка, в которую должна переместиться камера (например, центр уровня)
    public Vector3 cameraEndPosition = Vector3.zero;
    // Флаг окончания игры
    public bool isGameOver = false;

    // Ссылка на основную камеру
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

    // Метод, вызываемый когда игра заканчивается.
    // Параметр playerWon определяет, выиграл ли игрок (true) или проиграл (false).
    public void EndGame(bool playerWon)
    {
        if (isGameOver)
            return;

        isGameOver = true;

        // Остановить спавн юнитов — например, отключить объекты-спавнеры (не показано в этом скрипте)
        // Можно рассмотреть вариант отправки сообщения всем спавнерам, чтобы они прекратили работу.

        // Удаляем все существующие юниты
        UnitBase[] allUnits = FindObjectsOfType<UnitBase>();
        foreach (UnitBase unit in allUnits)
        {
            Destroy(unit.gameObject);
        }

        // Запускаем последовательность окончания игры:
        // сначала плавно перемещаем камеру, затем выводим сообщение с эффектом печати.
        StartCoroutine(EndGameSequence(playerWon));
    }

    IEnumerator EndGameSequence(bool playerWon)
    {
        // Плавное перемещение камеры от текущей позиции к заданной (например, центру)
        Vector3 startPos = mainCamera.transform.position;
        float elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(startPos, cameraEndPosition, elapsed / cameraMoveDuration);
            yield return null;
        }
        mainCamera.transform.position = cameraEndPosition;

        // После завершения движения камеры выводим сообщение
        string message = playerWon ? "Victory" : "Defeat";
        yield return StartCoroutine(TypewriterEffect(message));
    }

    // Эффект "печатающейся" строки, которая появляется по буквам
    IEnumerator TypewriterEffect(string message)
    {
        endGameText.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            endGameText.text += message[i];
            yield return new WaitForSeconds(0.1f); // задержка между появлением каждой буквы
        }
    }
}
