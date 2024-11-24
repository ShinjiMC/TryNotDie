using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public Image healthBar; // Imagen de la barra de vida circular
    public float maxHealth = 100f; // Salud máxima

    public GameObject playerObject; // Objeto del jugador que tiene el script de salud
    private float playerHealth; // Vida actual del jugador

    void Update()
    {
        // Obtenemos la vida del jugador desde otro objeto
        playerHealth = playerObject.GetComponent<PlayerHealth>().currentHealth;

        // Actualizamos la barra de vida
        UpdateHealthBar();
    }

    // Método para actualizar la barra de vida en cuatro secciones
    void UpdateHealthBar()
    {
        // Calculamos el porcentaje de vida actual
        float healthPercentage = playerHealth / maxHealth;

        // Si la vida está al 100%, no se muestra ninguna rotura
        if (healthPercentage >= 1f)
        {
            healthBar.fillAmount = 0f; // No mostrar nada si está al 100%
        }
        // Vida entre 88% y 99%, primera pequeña rotura
        else if (healthPercentage >= 0.875f)
        {
            healthBar.fillAmount = 0.125f; // Mostrar una pequeña rotura
        }
        // Vida entre 75% y 87%, segunda rotura
        else if (healthPercentage >= 0.75f)
        {
            healthBar.fillAmount = 0.25f; // Mostrar segunda rotura
        }
        // Vida entre 62% y 74%, tercera rotura
        else if (healthPercentage >= 0.625f)
        {
            healthBar.fillAmount = 0.375f; // Mostrar tercera rotura
        }
        // Vida entre 50% y 61%, cuarta rotura
        else if (healthPercentage >= 0.50f)
        {
            healthBar.fillAmount = 0.50f; // Mostrar cuarta rotura
        }
        // Vida entre 37% y 49%, quinta rotura
        else if (healthPercentage >= 0.375f)
        {
            healthBar.fillAmount = 0.625f; // Mostrar quinta rotura
        }
        // Vida entre 25% y 36%, sexta rotura
        else if (healthPercentage >= 0.25f)
        {
            healthBar.fillAmount = 0.75f; // Mostrar sexta rotura
        }
        // Vida entre 12% y 24%, séptima rotura
        else if (healthPercentage >= 0.125f)
        {
            healthBar.fillAmount = 0.875f; // Mostrar séptima rotura
        }
        // Vida entre 1% y 11%, octava rotura (parcial)
        else if (healthPercentage > 0f)
        {
            healthBar.fillAmount = 0.875f; // Mostrar la rotura casi completa (pero no completamente rota)
        }
        // Vida en 0%, mostrar la rotura completa
        else
        {
            healthBar.fillAmount = 1f; // Mostrar la rotura completa solo cuando la vida esté exactamente en 0
        }
    }


}
