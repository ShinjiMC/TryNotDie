using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedGame : MonoBehaviour
{
    public Light pointLight;          // Referencia al Point Light
    public FlashingLight flashingLightScript; // Referencia al script de luz intermitente
    public Vector3 lightPosition;     // Posición del Point Light cuando todas las partes están recolectadas
    public float lightIntensity = 1f; // Intensidad del Point Light cuando todas las partes están recolectadas
    public float indirectMultiplier = 1f; // Multiplicador indirecto de luz cuando todas las partes están recolectadas

    public GameObject alertSound;     // Objeto de sonido de alerta
    public GameObject button;          // Objeto botón que se activará
    public GameObject button2;         // Objeto botón que se activará
    public Vector3 buttonTargetPosition; // Nueva posición del botón a la que debe moverse

    // Método para actualizar las propiedades de la luz
    public void UpdateLightProperties()
    {
        if (pointLight != null)
        {
            pointLight.color = Color.white; // Cambia el color a blanco
            pointLight.transform.position = lightPosition; // Cambia la posición
            pointLight.intensity = lightIntensity; // Cambia la intensidad
            pointLight.bounceIntensity = indirectMultiplier; // Cambia el multiplicador indirecto

            if (flashingLightScript != null)
            {
                flashingLightScript.enabled = false; // Desactiva la luz intermitente
            }
            if (alertSound != null)
            {
                alertSound.SetActive(false); // Desactiva el sonido de alerta
            }
        }
    }

    // Método para activar y mover el botón
    public void ActivateAndMoveButton()
    {
        if (button != null)
        {
            button.SetActive(true); // Activa el botón
            StartCoroutine(MoveButtonToPosition(button.transform, buttonTargetPosition, 5f)); // Inicia la animación
        }
    }

    // Coroutine para mover el botón a la posición objetivo
    private IEnumerator MoveButtonToPosition(Transform buttonTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = buttonTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            buttonTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Espera un frame
        }

        buttonTransform.position = targetPosition; // Asegura que la posición final sea exacta
        button.SetActive(false); // Desactiva el botón después de la animación
        button2.SetActive(true); // Activa el segundo botón
    }
}
