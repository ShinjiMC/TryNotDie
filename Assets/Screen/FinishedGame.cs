using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedGame : MonoBehaviour
{
    public Light pointLight;          // Referencia al Point Light
    public FlashingLight flashingLightScript; // Referencia al script de luz intermitente
    public Vector3 lightPosition;     // Posici�n del Point Light cuando todas las partes est�n recolectadas
    public float lightIntensity = 1f; // Intensidad del Point Light cuando todas las partes est�n recolectadas
    public float indirectMultiplier = 1f; // Multiplicador indirecto de luz cuando todas las partes est�n recolectadas

    public GameObject alertSound;     // Objeto de sonido de alerta
    public GameObject button;          // Objeto bot�n que se activar�
    public GameObject button2;         // Objeto bot�n que se activar�
    public Vector3 buttonTargetPosition; // Nueva posici�n del bot�n a la que debe moverse

    // M�todo para actualizar las propiedades de la luz
    public void UpdateLightProperties()
    {
        if (pointLight != null)
        {
            pointLight.color = Color.white; // Cambia el color a blanco
            pointLight.transform.position = lightPosition; // Cambia la posici�n
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

    // M�todo para activar y mover el bot�n
    public void ActivateAndMoveButton()
    {
        if (button != null)
        {
            button.SetActive(true); // Activa el bot�n
            StartCoroutine(MoveButtonToPosition(button.transform, buttonTargetPosition, 5f)); // Inicia la animaci�n
        }
    }

    // Coroutine para mover el bot�n a la posici�n objetivo
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

        buttonTransform.position = targetPosition; // Asegura que la posici�n final sea exacta
        button.SetActive(false); // Desactiva el bot�n despu�s de la animaci�n
        button2.SetActive(true); // Activa el segundo bot�n
    }
}
