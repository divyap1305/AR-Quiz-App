using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (arCamera == null)
            return;

        Vector3 direction = transform.position - arCamera.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}