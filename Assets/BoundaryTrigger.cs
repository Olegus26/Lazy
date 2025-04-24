using UnityEngine;
using TMPro;

public class BoundaryTrigger : MonoBehaviour
{
    // Установи в инспекторе: true для левого края (например, база игрока), false для правого края (база врага)
    public bool isLeftBoundary;

    // Количество жизней для базы, к которой привязан этот триггер
    public int lives = 3;

    // Ссылка на UI-текст, показывающий оставшиеся жизни
    public TextMeshProUGUI livesText;

    private void Start()
    {
        UpdateLivesUI();
    }

    // Обновляем текст с жизнями
    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = lives.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Предполагаем, что на юнитах есть скрипт UnitBase с булевой переменной isEnemy
        UnitBase unit = collision.GetComponent<UnitBase>();
        if (unit != null)
        {
            // Если это левая граница (база игрока), то отнимаем жизнь, когда к ней добегает юнит врага (isEnemy == true)
            if (isLeftBoundary && unit.isEnemy)
            {
                lives--;
                UpdateLivesUI();
                Destroy(unit.gameObject);
                CheckGameOver();
            }
            // Если это правая граница (база врага), то отнимаем жизнь, когда к ней добегает юнит игрока (isEnemy == false)
            else if (!isLeftBoundary && !unit.isEnemy)
            {
                lives--;
                UpdateLivesUI();
                Destroy(unit.gameObject);
                CheckGameOver();
            }
        }
    }

    // Проверка условия окончания игры
    void CheckGameOver()
    {
        if (lives <= 0)
        {
            GameManager.Instance.EndGame(!isLeftBoundary);
        }
    }
}
