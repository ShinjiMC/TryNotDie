using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door; // Objeto de la puerta que se moverá
    public Barra bar;
    public Vector3 targetPosition; // Nueva posición de la puerta
    public Vector3 targetRotationEuler; // Nueva rotación de la puerta en Euler angles
    public Vector3 targetScale; // Nueva escala de la puerta
    public GameObject sparks;

    private Vector3 originalPosition; // Posición original de la puerta
    private Quaternion originalRotation; // Rotación original de la puerta
    private Vector3 originalScale; // Escala original de la puerta
    public bool status;
    void Start()
    {
        // Guardamos la posición, rotación y escala originales de la puerta
        originalPosition = door.transform.position;
        originalRotation = door.transform.rotation;
        originalScale = door.transform.localScale;
        status = false;
        sparks.SetActive(true);
    }

    void Update()
    {
        // Verificamos el estado inicial de la barra
        status = bar.GetStatuInitial();

        if (status)
        {
            sparks.SetActive(false);
            Quaternion targetRotation = Quaternion.Euler(targetRotationEuler);
            MoveDoor(targetPosition, targetRotation, targetScale);

        }
        else
        {
            sparks.SetActive(true);
            MoveDoor(originalPosition, originalRotation, originalScale);
        }
    }

    void MoveDoor(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // Movemos, rotamos y escalamos la puerta a las posiciones especificadas
        door.transform.position = position;
        door.transform.rotation = rotation;
        door.transform.localScale = scale; // Cambia la escala de la puerta
    }
}