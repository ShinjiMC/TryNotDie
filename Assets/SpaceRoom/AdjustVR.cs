using UnityEngine;

public class CanvasAlignerMultipleVR : MonoBehaviour
{
    public Camera vrCamera; // Asigna tu Main Camera aquí en el Inspector.
    public RectTransform[] uiElements; // Asigna aquí los tres RectTransforms en el Inspector.
    public float distanceFromCamera = 0.02f; // Distancia de los elementos a la cámara.
    public Vector3[] offsets; // Lista de offsets para cada RectTransform.

    void Start()
    {
        // Si no se ha asignado la cámara en el Inspector, buscar automáticamente la Main Camera
        if (vrCamera == null)
        {
            vrCamera = Camera.main;
        }

        // Si los offsets no están definidos, inicializarlos como Vector3(0, 0, 0) para cada elemento
        if (offsets == null || offsets.Length != uiElements.Length)
        {
            offsets = new Vector3[uiElements.Length];
            for (int i = 0; i < uiElements.Length; i++)
            {
                offsets[i] = Vector3.zero;
            }
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            // Alinear la posición de cada RectTransform frente a la cámara a una distancia específica
            Vector3 newElementPosition = vrCamera.transform.position + vrCamera.transform.forward * distanceFromCamera;

            // Aplicar el offset individual de cada elemento
            newElementPosition += offsets[i];

            // Asignar la nueva posición al elemento actual
            uiElements[i].position = newElementPosition;

            // Hacer que el elemento siempre mire hacia la cámara
            uiElements[i].rotation = Quaternion.LookRotation(uiElements[i].position - vrCamera.transform.position);

            // Mantener el elemento en la misma orientación que la cámara en el eje Y, pero sin rotación en los otros ejes (opcional)
            uiElements[i].eulerAngles = new Vector3(0, uiElements[i].eulerAngles.y, 0);
        }
    }
}
