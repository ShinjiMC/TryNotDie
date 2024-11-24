using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResizeOnGrab : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;  // Referencia al XRGrabInteractable
    public LineRenderer lineRenderer;  // Referencia al LineRenderer que deseas habilitar
    public GameObject glowPrefab;
    public GameObject pointLight;
    private bool hasBeenEnabledLine = false;  // Variable para comprobar si ya fue habilitado

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

        hasBeenEnabledLine = true;  // Marcar que ya ha sido habilitado
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
