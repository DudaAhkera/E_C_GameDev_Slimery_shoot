using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    public Transform target;           // Player
    public BoxCollider2D cameraBounds; // O objeto CameraBounds

    private float halfHeight;
    private float halfWidth;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        if (target == null || cameraBounds == null) return;

        Vector3 newPos = target.position;

        Bounds bounds = cameraBounds.bounds;

        newPos.x = Mathf.Clamp(newPos.x,
                               bounds.min.x + halfWidth,
                               bounds.max.x - halfWidth);

        newPos.y = Mathf.Clamp(newPos.y,
                               bounds.min.y + halfHeight,
                               bounds.max.y - halfHeight);

        newPos.z = transform.position.z;

        transform.position = newPos;
    }
}

