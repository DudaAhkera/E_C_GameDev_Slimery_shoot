using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [Header("Bounds")]
    public BoxCollider2D cameraBounds;

    private float halfHeight;
    private float halfWidth;

    private float minX, maxX, minY, maxY;

    void Start()
    {
        Camera cam = Camera.main;

        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        Bounds b = cameraBounds.bounds;

        minX = b.min.x + halfWidth;
        maxX = b.max.x - halfWidth;
        minY = b.min.y + halfHeight;
        maxY = b.max.y - halfHeight;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position, 
            new Vector3(clampedX, clampedY, transform.position.z), 
            smoothSpeed
        );

        transform.position = smoothedPosition;
    }
}

