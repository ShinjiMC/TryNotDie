using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.XR.OpenXR.Features.Interactions.HTCViveControllerProfile;

public class FixShip : MonoBehaviour
{
    public Collider triggerCollider; // Collider que actuará como trigger
    public Collider item1; // Objeto que se aceptará (trigger)
    public Collider item2; // Objeto que se aceptará (trigger)
    public Collider item3; // Objeto que se aceptará (trigger)

    public GameObject show1; // Objeto que se mostrará
    public GameObject show2; // Objeto que se mostrará
    public GameObject show3; // Objeto que se mostrará

    public GameObject smoke1; // Efecto de humo para el item1
    public GameObject smoke2; // Efecto de humo para el item2
    public GameObject smoke3; // Efecto de humo para el item3

    public VideoController videoController; // Referencia al script VideoController
    public Vector3 newPosition; // Nueva posición a la que se moverá el objeto
    public Status_Check statusCheck; // Referencia al script Status_Check

    public int acceptedItem = 1; // Número del objeto que se aceptará (1, 2 o 3)

    private void OnTriggerEnter(Collider other)
    {
        // Mensaje de depuración cuando un objeto entra en el trigger
        Debug.Log("Objeto entró en el trigger: " + other.gameObject.name);

        // Verifica si el objeto que entra al trigger es el aceptado según el valor de acceptedItem
        if ((acceptedItem == 1 && other == item1) ||
            (acceptedItem == 2 && other == item2) ||
            (acceptedItem == 3 && other == item3))
        {
            // Mensaje de depuración para identificar el item aceptado
            Debug.Log("Objeto aceptado: " + other.gameObject.name);

            // Destruye el objeto aceptado
            Destroy(other.gameObject);
            int itemN = 0;

            // Activa el objeto correspondiente a mostrar
            if (other == item1)
            {
                show1.SetActive(true);
                if (smoke1 != null)
                {
                    smoke1.SetActive(false); // Desactiva el humo1
                }
                itemN = 1;
            }
            else if (other == item2)
            {
                show2.SetActive(true);
                if (smoke2 != null)
                {
                    smoke2.SetActive(false); // Desactiva el humo2
                }
                itemN = 2;
            }
            else if (other == item3)
            {
                show3.SetActive(true);
                if (smoke3 != null)
                {
                    smoke3.SetActive(false); // Desactiva el humo3
                }
                itemN = 3;
            }

            // Inicia la corutina para mover y destruir el objeto
            StartCoroutine(MoveAndDestroy(itemN));
        }
        else
        {
            // Mensaje de depuración para identificar un objeto no aceptado
            Debug.Log("Objeto no aceptado: " + other.gameObject.name);
        }
    }

    private IEnumerator MoveAndDestroy(int itemN)
    {
        // Guarda la posición inicial del objeto
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float duration = 2f; // Duración del movimiento en segundos

        while (elapsedTime < duration)
        {
            // Interpolación lineal para mover el objeto
            transform.position = Vector3.Lerp(startPosition, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Espera un frame
        }

        // Asegura que llegue exactamente a la nueva posición
        transform.position = newPosition;

        if (statusCheck != null)
        {
            statusCheck.SetTrue();
        }

        videoController.SetPartCollected(itemN);

        // Destruye el objeto que contiene este script
        Destroy(gameObject);
    }
}
