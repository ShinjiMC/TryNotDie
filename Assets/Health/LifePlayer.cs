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
    public Image oxygenBar; // Imagen de la barra de ox�geno
    public Image oxygenBarBlack; // Imagen de la barra negra de ox�geno

    public float hungerRate = 1f; // Tasa a la que disminuye el hambre por segundo
    public float oxygenRate = 2f; // Tasa a la que disminuye el ox�geno por segundo
    public float healthDecreaseOnZero = 10f; // Cantidad de vida que se pierde al llegar a cero ox�geno/hambre
    public AudioSource warningSound;

    public EndScreen endScreen;
    void Start()
    {
        // Comenzamos las corrutinas para reducir el hambre y el ox�geno con el tiempo
        StartCoroutine(ReduceHungerOverTime());
        StartCoroutine(ReduceOxygenOverTime());
    }

    void Update()
    {
        // Actualizamos las barras de salud, hambre y ox�geno
        UpdateBars();
        ManageOxygenWarningSound();
    }

    // Funci�n para actualizar el fillAmount de las barras seg�n los valores actuales
    void UpdateBars()
    {
        hungryBar.fillAmount = currentHungry / maxHungry;
        hungryBarBlack.fillAmount = currentHungry / maxHungry;
        oxygenBar.fillAmount = currentOxygen / maxOxygen;
        oxygenBarBlack.fillAmount = currentOxygen / maxOxygen;
    }
    void ManageOxygenWarningSound()
    {
        if (currentOxygen < maxOxygen * 0.3f) // Si el ox�geno est� por debajo del 30%
        {
            warningSound.mute = false; // Activa el AudioSource
        }
        else // Si el ox�geno es 30% o m�s
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
                currentHungry = Mathf.Clamp(currentHungry, 0, maxHungry); // Limita el valor entre 0 y el m�ximo
            }
            else
            {
                TakeDamage(healthDecreaseOnZero); // Reducimos la salud
                yield return new WaitForSeconds(1f); // Espera un segundo antes de seguir restando salud
            }
        }
    }

    // Corrutina que reduce el ox�geno con el tiempo
    IEnumerator ReduceOxygenOverTime()
    {
        while (true) // Bucle infinito
        {
            if (currentOxygen > 0)
            {
                yield return new WaitForSeconds(1f); // Espera un segundo
                currentOxygen -= oxygenRate; // Reduce el ox�geno
                currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen); // Limita el valor entre 0 y el m�ximo
            }
            else
            {
                TakeDamage(healthDecreaseOnZero); // Reducimos la salud
                yield return new WaitForSeconds(1f); // Espera un segundo antes de seguir restando salud
            }
        }
    }

    // Funci�n p�blica para hacer da�o al jugador
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Reduce la salud
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asegura que la salud no sea negativa ni supere el m�ximo
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Aseg�rate de que no sea negativa
            endScreen.DeactivateObjects(); // Llama a DeactivateObjects en el script EndScreen
            endScreen.PlayVideo(); // Llama a PlayVideo en el script EndScreen
        }
    }

    // Funci�n p�blica para recuperar hambre
    public void RecoverHunger(float recoverAmount)
    {
        currentHungry += recoverAmount; // Aumenta el hambre
        currentHungry = Mathf.Clamp(currentHungry, 0, maxHungry); // Asegura que el hambre no supere el m�ximo ni sea negativa
    }

    // Funci�n p�blica para recargar ox�geno
    public void RecoverOxygen(float recoverAmount)
    {
        currentOxygen += recoverAmount; // Aumenta el ox�geno
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen); // Asegura que el ox�geno no supere el m�ximo ni sea negativo
    }
}
