using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResizeOnGrabAcid : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;  // Referencia al XRGrabInteractable
    public LineRenderer lineRenderer;  // Referencia al LineRenderer que deseas habilitar
    public GameObject glowPrefab;
    public GameObject pointLight;
    public GameObject objectToAscend; // Objeto que ascenderá
    public float targetHeight = 5f; // Altura objetivo a la que ascenderá el objeto
    private bool hasBeenEnabledLine = false;  // Variable para comprobar si ya fue habilitado
    public GameObject sound;

    void Start()
    {
        // Asegúrate de que el grab interactable esté configurado
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        // Verificar si se ha encontrado el XRGrabInteractable
        if (grabInteractable == null)
        {
            Debug.LogError("No se encontró el componente XRGrabInteractable en el objeto.");
            return;
        }

        // Suscribirse al evento cuando el objeto es agarrado
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    // Método que se llamará la primera vez que el objeto es agarrado
    private void OnGrab(SelectEnterEventArgs args)
    {
        // Solo habilitar el LineRenderer si no ha sido habilitado antes
        if (!hasBeenEnabledLine)
        {
            EnableLineRenderer();
        }
    }

    // Método para habilitar el LineRenderer
    private void EnableLineRenderer()
    {
        if (lineRenderer != null) // Verifica que el LineRenderer no sea nulo
        {
            lineRenderer.enabled = true;  // Habilitar el LineRenderer
        }

        if (glowPrefab != null)
        {
            glowPrefab.SetActive(false);
        }

        if (pointLight != null)
        {
            pointLight.SetActive(false);
        }
        else
        {
            Debug.LogError("El LineRenderer no está asignado.");
        }
        if (sound != null)
        {
            sound.SetActive(true);
        }
        hasBeenEnabledLine = true;  // Marcar que ya ha sido habilitado

        // Iniciar la corutina que espera 10 segundos y luego hace ascender el objeto
        StartCoroutine(AscendObjectAfterDelay(10f));
    }

    // Corutina que espera y hace ascender el objeto
    private IEnumerator AscendObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Esperar 10 segundos
        
        if (objectToAscend != null)
        {
            Vector3 startPosition = objectToAscend.transform.position;
            Vector3 targetPosition = new Vector3(startPosition.x, targetHeight, startPosition.z);

            float elapsedTime = 0f;
            float duration = 60f; // Tiempo total para ascender en segundos

            while (elapsedTime < duration)
            {
                objectToAscend.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null; // Esperar un frame
            }

            // Asegurarse de que el objeto llegue exactamente a la altura objetivo
            objectToAscend.transform.position = targetPosition;
        }
        else
        {
            Debug.LogError("El objeto a ascender no está asignado.");
        }
    }

    void OnDestroy()
    {
        // Desuscribirse del evento cuando el objeto es destruido
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}
