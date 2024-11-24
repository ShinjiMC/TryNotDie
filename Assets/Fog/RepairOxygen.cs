using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairOxygen : MonoBehaviour
{
    public PlayerHealth playerHealth; // Referencia al script PlayerHealth
    public AudioSource oxygenRestoreAudio; // Referencia al AudioSource para reproducir el sonido

    // Función pública para recuperar el oxígeno al 100%

    void Start()
    {
        if (playerHealth != null)
        {
            Debug.Log("PlayerHealth is not null");
        }

        if (oxygenRestoreAudio == null)
        {
            Debug.LogWarning("AudioSource for oxygen restore is not assigned!");
        }
    }

    public void RestoreFullOxygen()
    {
        if (playerHealth != null)
        {
            playerHealth.RecoverOxygen(playerHealth.maxOxygen); // Recupera el oxígeno al valor máximo

            if (oxygenRestoreAudio != null)
            {
                oxygenRestoreAudio.Play(); // Reproduce el sonido
            }
        }
    }
}
