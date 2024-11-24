using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public enum Axis { X, Y, Z } // Enum para seleccionar el eje de rotación
    public Axis rotationAxis1; // Eje de rotación para object1
    public Axis rotationAxis2; // Eje de rotación para object2

    public GameObject object1;
    public GameObject object2;
    public GameObject material1;
    public GameObject material2;
    public Color newColor;

    public float scaleSpeed = 1.5f; // Velocidad del cambio de tamaño
    public float scaleFactor = 1.2f; // Factor de escala para agrandar
    private bool isAccepted = false;

    // Tamaños originales
    private Vector3 originalScale1;
    private Vector3 originalScale2;

    // Velocidades de rotación
    public float fastRotationSpeed = 360f; // Velocidad de rotación rápida durante el efecto
    public float slowRotationSpeed = 180f; // Velocidad de rotación lenta después del efecto
    private float effectRotationDuration = 1f; // Duración del efecto de rotación

    private float currentRotationSpeed; // Velocidad de rotación actual

    void Start()
    {
        if (object1 == null || object2 == null)
        {
            Debug.LogError("No se han asignado los objetos a escalar.");
            return;
        }
        if (material1 == null || material2 == null)
        {
            Debug.LogError("No se han asignado los materiales a cambiar de color.");
            return;
        }
        if (newColor == null)
        {
            Debug.LogError("No se ha asignado el nuevo color.");
            return;
        }

        originalScale1 = object1.transform.localScale;
        originalScale2 = object2.transform.localScale;

        currentRotationSpeed = 0f; // Inicializar velocidad de rotación
    }

    void Update()
    {
        if (!isAccepted)
        {
            ScaleObjects();
        }
        else
        {
            // Hacer que el objeto gire constantemente
            RotateObjects(currentRotationSpeed);
        }
    }

    // Cambiar el tamaño constantemente para crear efecto de "latido"
    private void ScaleObjects()
    {
        if (object1 != null)
            ScaleObject(object1, originalScale1);
        if (object2 != null)
            ScaleObject(object2, originalScale2);
    }

    // Cambiar el tamaño de un objeto oscilando entre su tamaño original y agrandado
    private void ScaleObject(GameObject obj, Vector3 originalScale)
    {
        float scale = 1 + Mathf.Sin(Time.time * scaleSpeed) * (scaleFactor - 1);
        obj.transform.localScale = originalScale * scale;
    }

    // Función para activar el efecto Accepted
    public void Accepted()
    {
        StartCoroutine(AcceptedEffect());
    }

    // Efecto de cambio de color transicional en el Main Color
    private IEnumerator AcceptedEffect()
    {
        isAccepted = true;

        Renderer renderer1 = material1.GetComponent<Renderer>();
        Renderer renderer2 = material2.GetComponent<Renderer>();

        if (renderer1 == null || renderer2 == null)
        {
            Debug.LogError("No se encontró el componente Renderer en uno o ambos objetos de material.");
            isAccepted = false;
            yield break;
        }

        // Guardar el color original de los materiales
        Color originalColor1 = renderer1.material.GetColor("_Color");
        Color originalColor2 = renderer2.material.GetColor("_Color");

        // Cambiar el color de manera transicional
        float timer = 0f;

        // Duración de la transición de color y rotación rápida
        while (timer < effectRotationDuration)
        {
            // Interpolar el color hacia el nuevo color
            renderer1.material.SetColor("_Color", Color.Lerp(originalColor1, newColor, timer / effectRotationDuration));
            renderer2.material.SetColor("_Color", Color.Lerp(originalColor2, newColor, timer / effectRotationDuration));

            // Rotar alrededor de los ejes seleccionados con velocidad rápida
            RotateObjects(fastRotationSpeed);

            timer += Time.deltaTime;
            yield return null;
        }

        // Asegurar que el color final sea el nuevo color
        renderer1.material.SetColor("_Color", newColor);
        renderer2.material.SetColor("_Color", newColor);

        // Restaurar el tamaño original de los objetos
        if (object1 != null)
            object1.transform.localScale = originalScale1;
        if (object2 != null)
            object2.transform.localScale = originalScale2;

        // Suavizar la transición de velocidad de rotación
        float transitionDuration = 1f; // Duración de la transición de velocidad
        float transitionTimer = 0f;

        while (transitionTimer < transitionDuration)
        {
            currentRotationSpeed = Mathf.Lerp(fastRotationSpeed, slowRotationSpeed, transitionTimer / transitionDuration);
            RotateObjects(currentRotationSpeed);
            transitionTimer += Time.deltaTime;
            yield return null; // Esperar al siguiente frame
        }

        currentRotationSpeed = slowRotationSpeed; // Asegurarse de que la velocidad final sea la lenta

        // Después de la transición, mantener la rotación constante
        while (isAccepted)
        {
            RotateObjects(currentRotationSpeed);
            yield return null; // Esperar al siguiente frame
        }
    }

    // Rotar los objetos alrededor de sus ejes seleccionados
    private void RotateObjects(float speed)
    {
        object1.transform.Rotate(GetRotationAxis(rotationAxis1), speed * Time.deltaTime);
        object2.transform.Rotate(GetRotationAxis(rotationAxis2), speed * Time.deltaTime);
    }

    // Obtener el eje de rotación basado en el enum seleccionado
    private Vector3 GetRotationAxis(Axis axis)
    {
        switch (axis)
        {
            case Axis.X: return Vector3.right; // Eje X
            case Axis.Y: return Vector3.up;    // Eje Y
            case Axis.Z: return Vector3.forward; // Eje Z
            default: return Vector3.zero;
        }
    }
}
