using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform; // Ссылка на камеру
    public float parallaxEffect = 0.5f; // Сила эффекта параллакса (0 - не двигается, 1 - как камера)

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Если камера не задана, берём основную
        }
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, 0, 0); // Двигаем фон с замедлением
        lastCameraPosition = cameraTransform.position;
    }
}
