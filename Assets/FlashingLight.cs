using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    public Light alertLight;  // Referencia a la luz
    public float flashSpeed = 1f;  // Velocidad del parpadeo
    private bool isOn = true;

    void Update()
    {
        // Alternar la luz entre encendido y apagado
        if (Time.time % (1 / flashSpeed) < (0.5 / flashSpeed))
        {
            if (!isOn)
            {
                alertLight.enabled = true;
                isOn = true;
            }
        }
        else
        {
            if (isOn)
            {
                alertLight.enabled = false;
                isOn = false;
            }
        }
    }
}
