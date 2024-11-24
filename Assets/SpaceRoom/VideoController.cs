using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Referencia al componente VideoPlayer
    public VideoClip videoAllPartsMissing; // Faltan todas las piezas
    public VideoClip videoMissing1and2;    // Faltan 1 y 2
    public VideoClip videoMissing1and3;    // Faltan 1 y 3
    public VideoClip videoMissing2and3;    // Faltan 2 y 3
    public VideoClip videoMissing1;        // Falta 1
    public VideoClip videoMissing2;        // Falta 2
    public VideoClip videoMissing3;        // Falta 3
    public VideoClip videoClipCompleted;   // Video de completado

    public Light pointLight;          // Referencia al Point Light
    public FlashingLight flashingLightScript;
    public Vector3 lightPosition;     // Posición del Point Light cuando todas las partes están recolectadas
    public float lightIntensity = 1f; // Intensidad del Point Light cuando todas las partes están recolectadas
    public float indirectMultiplier = 1f; // Multiplicador indirecto de luz cuando todas las partes están recolectadas

    public GameObject alertSound;
    public GameObject button;     // Objeto botón que se activará
    public GameObject button2;     // Objeto botón que se activará
    public Vector3 buttonTargetPosition; // Nueva posición del botón a la que debe moverse


    private bool part1Collected = false; // Estado de recolección de la parte 1
    private bool part2Collected = false; // Estado de recolección de la parte 2
    private bool part3Collected = false; // Estado de recolección de la parte 3

    private bool allPartsCollected = false;  // Esto será true cuando todas las partes estén recolectadas

    void Start()
    {
        // Iniciar la reproducción del video inicial
        UpdateVideoPlayer();
    }

    public void SetPartCollected(int partNumber)
    {
        // Marca la parte correspondiente como recolectada
        switch (partNumber)
        {
            case 1:
                part1Collected = true;
                break;
            case 2:
                part2Collected = true;
                break;
            case 3:
                part3Collected = true;
                break;
        }

        // Comprueba si todas las partes han sido recolectadas
        if (part1Collected && part2Collected && part3Collected)
        {
            allPartsCollected = true;
            UpdateLightProperties();
            ActivateAndMoveButton();
        }
        UpdateVideoPlayer();
    }

    private void UpdateVideoPlayer()
    {
        // Si todas las partes están recolectadas, reproduce el video de completado en bucle
        if (allPartsCollected)
        {
            videoPlayer.clip = videoClipCompleted;
            videoPlayer.isLooping = true; // Habilita el bucle para el video de completado
            videoPlayer.Play();
            return;
        }

        // Verifica qué partes faltan y selecciona el video apropiado
        if (!part1Collected && !part2Collected && !part3Collected)
        {
            videoPlayer.clip = videoAllPartsMissing;
        }
        else if (!part1Collected && !part2Collected)
        {
            videoPlayer.clip = videoMissing1and2;
        }
        else if (!part1Collected && !part3Collected)
        {
            videoPlayer.clip = videoMissing1and3;
        }
        else if (!part2Collected && !part3Collected)
        {
            videoPlayer.clip = videoMissing2and3;
        }
        else if (!part1Collected)
        {
            videoPlayer.clip = videoMissing1;
        }
        else if (!part2Collected)
        {
            videoPlayer.clip = videoMissing2;
        }
        else if (!part3Collected)
        {
            videoPlayer.clip = videoMissing3;
        }

        // Reproduce el video seleccionado en bucle
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    private void UpdateLightProperties()
    {
        if (pointLight != null)
        {
            pointLight.color = Color.white; // Cambia el color a blanco
            pointLight.transform.position = lightPosition; // Cambia la posición
            pointLight.intensity = lightIntensity; // Cambia la intensidad
            pointLight.bounceIntensity = indirectMultiplier; // Cambia el multiplicador indirecto
            if (flashingLightScript != null)
            {
                flashingLightScript.enabled = false;
            }
            if (alertSound != null)
            {
                alertSound.SetActive(false);
            }
        }
    }

    private void ActivateAndMoveButton()
    {
        if (button != null)
        {
            button.SetActive(true); // Activa el botón
            StartCoroutine(MoveButtonToPosition(button.transform, buttonTargetPosition, 5f)); // Inicia la animación
            
        }
    }

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
        button2.SetActive(true); // Activa el botón
    }
}
