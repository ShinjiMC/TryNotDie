using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Barra : MonoBehaviour
{
    public Effect effect0; // Primer script Effect
    public Effect effect1; // Segundo script Effect
    public Effect effect2; // Tercer script Effect
    public Effect effect3; // Cuarto script Effect

    private bool status0 = false; // Estado del primer script
    private bool status1 = false; // Estado del segundo script
    private bool status2 = false; // Estado del tercer script
    private bool status3 = false; // Estado del cuarto script

    public Image fillImage; // Imagen para el fill amount
    public TextMeshProUGUI percentageText; // Texto para mostrar el porcentaje
    public FinishedGame finishedGame; // Script FinishedGame
    // Duración de la animación
    public float animationDuration = 0.5f;
    public bool GetStatuInitial()
    {
        return status0;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI(); // Actualizar la UI al inicio
    }

    // Función pública para establecer el estado
    public void SetStatus(int number)
    {
        StartCoroutine(SetStatusCoroutine(number)); // Llamar a la corutina
    }

    // Coroutine para establecer el estado con espera
    private IEnumerator SetStatusCoroutine(int number)
    {
        // Esperar 2 segundos antes de proceder
        yield return new WaitForSeconds(2f);

        switch (number)
        {
            case 0:
                status0 = true;
                effect0.Accepted();
                break;
            case 1:
                status1 = true;
                effect1.Accepted();
                break;
            case 2:
                status2 = true;
                effect2.Accepted();
                break;
            case 3:
                status3 = true;
                effect3.Accepted();
                break;
            default:
                Debug.LogError("Número inválido. Debe ser entre 0 y 3.");
                yield break; // Salir si el número es inválido
        }

        UpdateUI(); // Actualizar la UI después de cambiar el estado
    }

    // Actualiza la UI en base a los estados
    private void UpdateUI()
    {
        // Contar cuántos estados son verdaderos
        int activeCount = 0;

        if (status0) activeCount++;
        if (status1) activeCount++;
        if (status2) activeCount++;
        if (status3) activeCount++;

        // Calcular el porcentaje y fillAmount
        float targetPercentage = (float)activeCount / 4 * 100; // 0% a 100%
        float targetFillAmount = (float)activeCount / 4; // 0 a 1

        // Iniciar la animación
        StartCoroutine(AnimateUI(targetPercentage, targetFillAmount));
        if (status0 && status1 && status2 && status3)
        {
            finishedGame.UpdateLightProperties(); // Actualiza las propiedades de la luz
            finishedGame.ActivateAndMoveButton(); // Activa y mueve el botón
        }
    }

    // Coroutine para animar el cambio de UI
    private IEnumerator AnimateUI(float targetPercentage, float targetFillAmount)
    {
        // Guardar valores iniciales
        float currentPercentage = float.Parse(percentageText.text.Replace("%", "")); // Obtener el porcentaje actual
        float currentFillAmount = fillImage.fillAmount; // Obtener el fillAmount actual
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolación
            float t = elapsedTime / animationDuration;

            // Actualizar el porcentaje y fillAmount
            float newPercentage = Mathf.Lerp(currentPercentage, targetPercentage, t);
            fillImage.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);

            // Actualizar el texto
            percentageText.text = $"{newPercentage:F0}%"; // Formatear a entero

            yield return null; // Esperar al siguiente frame
        }

        // Asegurarse que los valores finales son los correctos
        fillImage.fillAmount = targetFillAmount;
        percentageText.text = $"{targetPercentage:F0}%"; // Formatear a entero
    }
}
