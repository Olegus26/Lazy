using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform; // ������ �� ������
    public float parallaxEffect = 0.5f; // ���� ������� ���������� (0 - �� ���������, 1 - ��� ������)

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // ���� ������ �� ������, ���� ��������
        }
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, 0, 0); // ������� ��� � �����������
        lastCameraPosition = cameraTransform.position;
    }
}
