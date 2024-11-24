using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Para manejar NavMeshAgents

public class Acid_Player : MonoBehaviour
{
    public List<GameObject> objectsToMoveY; // Lista de objetos que se mover�n en Y
    public List<NavMeshAgent> navMeshAgentsToMoveY; // Lista de NavMeshAgents para los objetos peque�os
    public List<Rikayon> rikayonAgentsToMoveY; // Lista de NavMeshAgents para los objetos peque�os

    public GameObject objectToJump; // Objeto gigante que realizar� el salto
    public NavMeshAgent navMeshAgentForJump; // NavMeshAgent para el objeto gigante
    public Rikayon rikayonAgentForJump; // NavMeshAgent para el objeto gigante

    public GameObject alaObject; // Objeto de ala que se desactivar� tras el salto
    public GameObject alaHumo; // Objeto de ala que se desactivar� tras el salto
    public GameObject brilloObject; // Objeto de brillo que se activar� tras el salto

    public Vector3 targetJumpPosition; // Posici�n final del salto en Z
    public float jumpHeight = 5f; // Altura m�xima del salto
    public Collider playerCollider; // El collider del jugador
    public Collider triggerCollider; // El trigger que activar� el evento
    public float moveYAmount = 3f; // Valor que se mover�n en Y

    private bool isTriggered = false; // Para evitar que se ejecute varias veces

    void Start()
    {
        if (triggerCollider == null)
        {
            Debug.LogError("El triggerCollider no est� asignado.");
        }

        // Asegurarse de que los objetos ala y brillo est�n correctamente configurados
        if (alaObject != null)
        {
            alaObject.SetActive(true); // Asegurarse de que el ala est� activo al principio
        }
        if (alaHumo != null)
        {
            alaHumo.SetActive(true); // Asegurarse de que el ala est� activo al principio
        }
        if (brilloObject != null)
        {
            brilloObject.SetActive(false); // Asegurarse de que el brillo est� inactivo al principio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el jugador ha entrado en el trigger
        if (other == playerCollider && !isTriggered)
        {
            Debug.Log("El jugador ha llegado al trigger."); // Mensaje de debug

            isTriggered = true;

            // Desactivar NavMeshAgents de los objetos peque�os y activarlos para animaci�n
            foreach (NavMeshAgent agent in navMeshAgentsToMoveY)
            {
                if (agent != null)
                {
                    agent.enabled = false; // Desactivar el NavMeshAgent para permitir la animaci�n manual
                }
            }

            foreach (Rikayon agent in rikayonAgentsToMoveY)
            {
                if (agent != null)
                {
                    agent.enabled = false; // Desactivar el Rikayon para permitir la animaci�n manual
                }
            }

            // Desactivar NavMeshAgent del gigante
            if (navMeshAgentForJump != null)
            {
                navMeshAgentForJump.enabled = false; // Desactivar para permitir la animaci�n manual del salto
            }
            if (rikayonAgentForJump != null)
            {
                rikayonAgentForJump.enabled = false; // Desactivar para permitir la animaci�n manual del salto
            }

            // Activar los objetos que deben moverse
            foreach (GameObject obj in objectsToMoveY)
            {
                if (obj != null)
                {
                    obj.SetActive(true); // Activar el objeto
                }
            }

            StartCoroutine(MoveObjectsInY());
            StartCoroutine(AnimateJump());
        }
    }

    // Corutina para mover los objetos en Y por 5 segundos
    private IEnumerator MoveObjectsInY()
    {
        float duration = 5f; // Duraci�n de la animaci�n
        float elapsedTime = 0f;

        // Guardar las posiciones iniciales
        List<Vector3> initialPositions = new List<Vector3>();
        foreach (GameObject obj in objectsToMoveY)
        {
            initialPositions.Add(obj.transform.position);
        }

        while (elapsedTime < duration)
        {
            for (int i = 0; i < objectsToMoveY.Count; i++)
            {
                if (objectsToMoveY[i] != null)
                {
                    // Mover en Y interpolando desde la posici�n inicial
                    objectsToMoveY[i].transform.position = initialPositions[i] + new Vector3(0f, Mathf.Lerp(0f, moveYAmount, elapsedTime / duration), 0f);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null; // Esperar un frame
        }

        // Asegurar que los objetos lleguen exactamente al valor final
        for (int i = 0; i < objectsToMoveY.Count; i++)
        {
            if (objectsToMoveY[i] != null)
            {
                objectsToMoveY[i].transform.position = initialPositions[i] + new Vector3(0f, moveYAmount, 0f);
            }
        }

        // Volver a activar los NavMeshAgents para que los objetos peque�os sigan su ruta
        foreach (NavMeshAgent agent in navMeshAgentsToMoveY)
        {
            if (agent != null)
            {
                agent.enabled = true; // Reactivar NavMeshAgent
            }
        }

        foreach (Rikayon agent in rikayonAgentsToMoveY)
        {
            if (agent != null)
            {
                agent.enabled = true; // Reactivar NavMeshAgent
            }
        }
    }

    // Corutina para hacer que un objeto "salte" a una posici�n designada
    private IEnumerator AnimateJump()
    {
        if (objectToJump == null)
        {
            Debug.LogError("El objeto para saltar no est� asignado.");
            yield break;
        }

        float duration = 3f; // Duraci�n de la animaci�n de salto
        float elapsedTime = 0f;

        Vector3 startPosition = objectToJump.transform.position;
        Vector3 endPosition = targetJumpPosition;

        // Calcular la distancia horizontal total (en X y Z) que debe recorrer
        Vector3 horizontalDistance = endPosition - startPosition;
        horizontalDistance.y = 0; // No queremos incluir la altura (Y) en el c�lculo de la distancia horizontal

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Movimiento horizontal interpolando entre la posici�n inicial y la posici�n final (X y Z)
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, endPosition, t);

            // Movimiento vertical creando el efecto de un salto (par�bola)
            float yPosition = Mathf.Lerp(startPosition.y, endPosition.y, t) + jumpHeight * Mathf.Sin(t * Mathf.PI);

            // Actualizar la posici�n combinando movimiento en X, Y y Z
            objectToJump.transform.position = new Vector3(horizontalPosition.x, yPosition, horizontalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; // Esperar un frame
        }

        // Asegurar que el objeto llegue exactamente a la posici�n final
        objectToJump.transform.position = targetJumpPosition;

        // Reactivar el NavMeshAgent del gigante para que contin�e su ruta
        if (navMeshAgentForJump != null)
        {
            navMeshAgentForJump.enabled = true; // Reactivar NavMeshAgent
        }
        if (rikayonAgentForJump != null)
        {
            rikayonAgentForJump.enabled = true; // Reactivar NavMeshAgent
        }

        // Activar el objeto brillo y desactivar el objeto ala al final de la animaci�n
        if (alaObject != null)
        {
            alaObject.SetActive(false); // Desactivar el objeto ala
        }
        if(alaHumo !=null)
        {
            alaHumo.SetActive(false);
        }
        if (brilloObject != null)
        {
            brilloObject.SetActive(true); // Activar el objeto brillo
        }
    }
}
