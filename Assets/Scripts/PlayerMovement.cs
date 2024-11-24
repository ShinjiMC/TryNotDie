using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;
    public Rigidbody rb;
    public Transform cameraTransform;  // Referencia a la cámara

    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Evita que el Rigidbody se gire de forma no deseada

        // Oculta el cursor y lo bloquea al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        RotateCamera();
    }

    // Función para mover al jugador
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");  // Movimiento izquierda/derecha (A/D)
        float moveZ = Input.GetAxis("Vertical");    // Movimiento adelante/atrás (W/S)

        Vector3 move = transform.right * moveX + transform.forward * moveZ;  // Dirección del movimiento
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);  // Movimiento del Rigidbody
    }

    // Función para rotar la cámara en primera persona con el ratón
    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;  // Controla la rotación vertical
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limita la rotación vertical entre -90 y 90 grados

        // Aplica la rotación de la cámara en los ejes correspondientes
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);  
        transform.Rotate(Vector3.up * mouseX);  // Rotación del jugador en el eje Y
    }
}
