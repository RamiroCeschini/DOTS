using UnityEngine;

public class SimpleOrbitCameraFixed : MonoBehaviour
{
    public Transform pivot;
    public float rotationSpeed = 200f;
    public float zoomSpeed = 200f;      // ajustable
    public float minDistance = 10f;
    public float maxDistance = 800f;    // importante: poner algo mayor o igual a tu distancia actual

    float distance;

    void Start()
    {
        if (pivot == null)
        {
            Debug.LogError("Asigná un pivot!");
            enabled = false;
            return;
        }

        // Distancia inicial real entre cámara y pivot
        distance = Vector3.Distance(transform.position, pivot.position);
        // Aseguramos que los límites contengan la distancia inicial:
        minDistance = Mathf.Min(minDistance, distance);
        maxDistance = Mathf.Max(maxDistance, distance);
    }

    void Update()
    {
        // Rotación simple (derecho + mouse X)
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(pivot.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        }

        // ZOOM: movimiento AL PIVOT (infalible)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            // Calculamos nueva distancia (clampear la distancia, no la posición tentativa)
            float targetDistance = distance - scroll * zoomSpeed * Time.deltaTime;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

            // Si hay cambio, aplicamos nueva posición manteniendo la dirección actual
            if (!Mathf.Approximately(targetDistance, distance))
            {
                Vector3 dir = (transform.position - pivot.position).normalized;
                transform.position = pivot.position + dir * targetDistance;
                distance = targetDistance;
            }
        }
    }
}