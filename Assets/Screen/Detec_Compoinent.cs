using System.Collections;
using UnityEngine;

public class Detec_Component : MonoBehaviour
{
    public BoxCollider cajaCollider; // Box Collider trigger de Caja
    public BoxCollider componentCollider; // Box Collider trigger de Component
    public GameObject componentToShow; // Objeto Component a mostrar
    public GameObject smokeToHide; // Objeto Smoke a ocultar
    public Barra barraScript; // Script Barra
    public int number; // Número a pasar al script Barra

    public GameObject scanObject; // Objeto Scan a activar
    public float moveYTarget = 0.25f; // Valor de movimiento en el eje Y para el objeto Scan
    public float moveYInitial = -0.1f; // Posición Y inicial del objeto Scan
    public float duration = 2f; // Duración total del movimiento

    public Rigidbody componentRigidbody; // Rigidbody del objeto Component

    private bool isComponentInside = false; // Estado para verificar si el component está dentro del cajaCollider

    private void OnTriggerEnter(Collider other)
    {
        // Mensaje de depuración cuando un objeto entra en el trigger
        Debug.Log("Objeto entró en el trigger: " + other.gameObject.name);

        // Verifica si el objeto que entra al trigger es el component
        if (other == componentCollider)
        {
            // Mensaje de depuración para identificar el objeto Component
            Debug.Log("Component detectado: " + other.gameObject.name);

            // Activa el objeto Scan
            if (scanObject != null)
            {
                scanObject.SetActive(true);
                StartCoroutine(StartScanActivation()); // Inicia el proceso de escaneo
            }

            // Mueve el objeto Component al centro del collider y congela su Rigidbody
            MoveAndFreezeComponent(other.gameObject);

            // Muestra el componente deseado
            if (componentToShow != null)
            {
                componentToShow.SetActive(true);
            }

            // Desactiva el objeto de humo
            if (smokeToHide != null)
            {
                smokeToHide.SetActive(false);
            }
            if (barraScript != null)
            {
                barraScript.SetStatus(number);
            }
        }
        else
        {
            // Mensaje de depuración para identificar un objeto no aceptado
            Debug.Log("Objeto no aceptado: " + other.gameObject.name);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el componente está completamente dentro del cajaCollider
        if (other == componentCollider && isComponentInside)
        {
            StartCoroutine(MoveScanObject());
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el componente sale del primer collider, resetea el estado
        if (other == componentCollider)
        {
            isComponentInside = false;
        }
    }

    private IEnumerator StartScanActivation()
    {
        // Marca que el componente está dentro del cajaCollider
        isComponentInside = true;

        yield return null; // Espera un frame para asegurarse de que todo esté activo
    }

    private void MoveAndFreezeComponent(GameObject component)
    {
        // Mueve el objeto al centro del cajaCollider
        Vector3 center = cajaCollider.bounds.center;
        component.transform.position = center;

        // Congela la rotación y posición en todos los ejes
        if (componentRigidbody != null)
        {
            componentRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
    }

    private IEnumerator MoveScanObject()
    {
        // Posición inicial y final para el objeto Scan
        Vector3 originalPosition = new Vector3(scanObject.transform.localPosition.x, moveYInitial, scanObject.transform.localPosition.z);
        Vector3 targetPosition = new Vector3(originalPosition.x, moveYTarget, originalPosition.z);

        // Mover el objeto Scan hacia arriba
        float timer = 0f;

        while (timer < duration)
        {
            scanObject.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que la posición final sea la deseada
        scanObject.transform.localPosition = targetPosition;

        // Mover el objeto Scan hacia abajo
        timer = 0f;
        while (timer < duration)
        {
            scanObject.transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que la posición final sea la original
        scanObject.transform.localPosition = originalPosition;
        // Llama a SetStatus en el script Barra
        
        // Elimina el objeto Component
        if (componentCollider != null) // Verifica si componentCollider no es nulo antes de destruir
        {
            Destroy(componentCollider.gameObject); // Elimina el objeto component
        }

        

        // Desactiva el objeto Scan
        scanObject.SetActive(false); // Desactiva el objeto inmediatamente
    }
}
