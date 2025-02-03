using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 15f;

    private Vector3 dragOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ПКМ для перемещения
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += difference;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
    }
}
