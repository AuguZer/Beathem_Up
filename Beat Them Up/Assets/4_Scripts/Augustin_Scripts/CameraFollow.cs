using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //FOLLOW
    [SerializeField] Transform target;
    [SerializeField] float cameraSpeed = 5f;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] bool canFollow;

    Camera _camera;

    //BOUNDS LIMITE
    [SerializeField] BoxCollider2D cameraBounds;
    Vector2 cameraDimensions;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        cameraDimensions.x = _camera.orthographicSize * _camera.aspect;
        cameraDimensions.y = _camera.orthographicSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        canFollow = true;
        Vector3 followPos = target.position + offset;

        float minX = cameraBounds.transform.position.x - cameraBounds.size.x / 2 + cameraDimensions.x;
        float maxX = cameraBounds.transform.position.x + cameraBounds.size.x / 2 - cameraDimensions.x;

        followPos.x = Mathf.Clamp(followPos.x, minX, maxX);

        float minY = cameraBounds.transform.position.y - cameraBounds.size.y / 2 + cameraDimensions.y;
        float maxY = cameraBounds.transform.position.y + cameraBounds.size.y / 2 - cameraDimensions.y;

        followPos.y = Mathf.Clamp(followPos.y, minY, maxY);

        Vector3 currentVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, followPos, ref currentVelocity, Time.deltaTime * cameraSpeed);
        
        if (!canFollow)
        {
            return;
        }
    }
}
