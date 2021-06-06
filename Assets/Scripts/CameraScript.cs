using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    const int BASE_ZOOM = 5;

    Camera cameraComponent;

    void Start()
    {
        cameraComponent = gameObject.GetComponent<Camera>();
        cameraComponent.orthographicSize = BASE_ZOOM;
    }

    void UpdateZoom(float zoomValue)
    {
        //if (cameraComponent.orthographicSize)
        if (cameraComponent.orthographicSize - zoomValue >= 1)
        {
            cameraComponent.orthographicSize -= zoomValue;
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * 10 * Time.deltaTime;
        float moveY = Input.GetAxisRaw("Vertical") * 10 * Time.deltaTime;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x + moveX, gameObject.transform.position.y + moveY, gameObject.transform.position.z);

        float zoomValue = Input.mouseScrollDelta.y;
        UpdateZoom(zoomValue);
    }
}
