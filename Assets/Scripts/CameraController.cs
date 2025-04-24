using UnityEngine;
using UnityEngine.EventSystems; // Нужно для проверки клика по UI

public class CameraController : MonoBehaviour
{
    public float minX = -5f;
    public float maxX = 5f;
    public float speed = 0.1f;

    private Vector3 lastTouchPosition;
    private bool isDragging = false;

    void Update()
    {
        if (IsPointerOverUI()) return; // Если клик по UI, не двигаем камеру

        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 delta = Input.mousePosition - lastTouchPosition;
            float moveX = -delta.x * speed * Time.deltaTime;

            Vector3 newPosition = transform.position + new Vector3(moveX, 0, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            transform.position = newPosition;
            lastTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
