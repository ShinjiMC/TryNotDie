using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxHungry = 100f;
    public float maxOxygen = 100f;

    public float currentHealth = 100f;
    public float currentHungry = 100f;
    public float currentOxygen = 100f;

    public Image hungryBar; // Imagen de la barra de hambre
    public Image hungryBarBlack; // Imagen de la barra negra de hambre
    public Image oxygenBar; // Imagen de la barra de oxígeno
    public Image oxygenBarBlack; // Imagen de la barra negra de oxígeno

    public float hungerRate = 1f; // Tasa a la que disminuye el hambre por segundo
    public float oxygenRate = 2f; // Tasa a la que disminuye el oxígeno por segundo
    public float healthDecreaseOnZero = 10f; // Cantidad de vida que se pierde al llegar a cero oxígeno/hambre
    public AudioSource warningSound;

    public EndScreen endScreen;
    void Start()
    {
        // Comenzamos las corrutinas para reducir el hambre y el oxígeno con el tiempo
        StartCoroutine(ReduceHungerOverTime());
        StartCoroutine(ReduceOxygenOverTime());
    }

    void Update()
    {
        // Actualizamos las barras de salud, hambre y oxígeno
        UpdateBars();
        ManageOxygenWarningSound();
    }

    // Función para actualizar el fillAmount de las barras según los valores actuales
    void UpdateBars()
    {
        hungryBar.fillAmount = currentHungry / maxHungry;
        hungryBarBlack.fillAmount = currentHungry / maxHungry;
        oxygenBar.fillAmount = currentOxygen / maxOxygen;
        oxygenBarBlack.fillAmount = currentOxygen / maxOxygen;
    }
    void ManageOxygenWarningSound()
    {
        if (currentOxygen < maxOxygen * 0.3f) // Si el oxígeno está por debajo del 30%
        {
            warningSound.mute = false; // Activa el AudioSource
        }
        else // Si el oxígeno es 30% o más
        {
            warningSound.mute = true; // Desactiva el AudioSource
        }
    }
    // Corrutina que reduce el hambre con el tiempo
    IEnumerator ReduceHungerOverTime()
    {
        while (true) // Bucle infinito
        {
            if (currentHungry > 0)
            {
                yield return new WaitForSeconds(1f); // Espera un segundo
                currentHungry -= hungerRate; // Reduce el hambre
                currentHungry = Mathf.Clamp(currentHungry, 0, maxHungry); // Limita el valor entre 0 y el máximo
            }
            else
            {
                TakeDamage(healthDecreaseOnZero); // Reducimos la salud
                yield return new WaitForSeconds(1f); // Espera un segundo antes de seguir restando salud
            }
        }
    }

    // Corrutina que reduce el oxígeno con el tiempo
    IEnumerator ReduceOxygenOverTime()
    {
        while (true) // Bucle infinito
        {
            if (currentOxygen > 0)
            {
                yield return new WaitForSeconds(1f); // Espera un segundo
                currentOxygen -= oxygenRate; // Reduce el oxígeno
                currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen); // Limita el valor entre 0 y el máximo
            }
            else
            {
                TakeDamage(healthDecreaseOnZero); // Reducimos la salud
                yield return new WaitForSeconds(1f); // Espera un segundo antes de seguir restando salud
            }
        }
    }

    // Función pública para hacer daño al jugador
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Reduce la salud
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asegura que la salud no sea negativa ni supere el máximo
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Asegúrate de que no sea negativa
            endScreen.DeactivateObjects(); // Llama a DeactivateObjects en el script EndScreen
            endScreen.PlayVideo(); // Llama a PlayVideo en el script EndScreen
        }
    }

    // Función pública para recuperar hambre
    public void RecoverHunger(float recoverAmount)
    {
        currentHungry += recoverAmount; // Aumenta el hambre
        currentHungry = Mathf.Clamp(currentHungry, 0, maxHungry); // Asegura que el hambre no supere el máximo ni sea negativa
    }

    // Función pública para recargar oxígeno
    public void RecoverOxygen(float recoverAmount)
    {
        currentOxygen += recoverAmount; // Aumenta el oxígeno
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen); // Asegura que el oxígeno no supere el máximo ni sea negativo
    }
}
