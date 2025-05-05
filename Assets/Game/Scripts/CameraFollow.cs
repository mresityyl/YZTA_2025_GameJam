using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private Bounds _cameraBounds;
    private Vector3 targetPosition;
    private Camera _mainCamera;

    [SerializeField] private Transform target;

    void Awake()
    {
        _mainCamera = Camera.main;
    }
    void Start()
    {
        var height = _mainCamera.orthographicSize;
        var width = height * _mainCamera.aspect;

        var minX = Globals.worldBounds.min.x + width;
        var maxX = Globals.worldBounds.extents.x - width;

        var minY = Globals.worldBounds.min.y + height;
        var maxY = Globals.worldBounds.extents.y - height;

        _cameraBounds = new Bounds();
        _cameraBounds.SetMinMax(
            new Vector3(minX, minY, 0.0f),
            new Vector3(maxX, maxY, 0.0f)
        );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPosition = target.position + offset;
        targetPosition = GetCameraBounds();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    private Vector3 GetCameraBounds()
    {
        return new Vector3(
                Mathf.Clamp(targetPosition.x, _cameraBounds.min.x, _cameraBounds.max.x),
                Mathf.Clamp(targetPosition.y, _cameraBounds.min.y, _cameraBounds.max.y),
                transform.position.z
            );
    }
}
