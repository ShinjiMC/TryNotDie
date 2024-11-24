using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : MonoBehaviour
{
    public float damage = 10f; // Daño que hará el cangrejo
    public float attackRate = 1f; // Frecuencia de ataques (1 ataque por segundo)
    private float nextAttackTime = 0f; // Tiempo para el próximo ataque
    private Animator animator; // Referencia al Animator del cangrejo

    public Collider playerCollider; // Collider del jugador (no trigger)
    public Collider visionCollider; // Collider de visión (trigger)
    public GameObject[] eyes; // Ojos del cangrejo (materiales que cambiarán de color)
    public Color farColor = Color.yellow; // Color cuando el jugador está a una distancia media
    public Color closeColor = Color.red; // Color cuando el jugador está cerca
    public Color normalColor = Color.white; // Color por defecto de los ojos del cangrejo

    public GameObject movementObject; // Objeto que contiene el script de Rikayon
    private Rikayon movementScript; // Referencia al script de movimiento
    public GameObject playerHealthObject; // Objeto que contiene el script PlayerHealth
    private PlayerHealth playerHealth; // Referencia al script PlayerHealth
    private bool isPlayerInRange = false; // Nuevo indicador para saber si el jugador está en rango

    public float defaultSpeed = 3.5f; // Velocidad predeterminada
    public float slowSpeed = 1.5f; // Velocidad cuando los ojos son amarillos
    public float fastSpeed = 5.0f;

    void Start()
    {
        // Asigna el script de movimiento desde el objeto que lo contiene
        if (movementObject != null)
        {
            movementScript = movementObject.GetComponent<Rikayon>();
            animator = movementObject.GetComponent<Animator>();
        }

        // Verifica si se encontró el script
        if (movementScript == null)
        {
            Debug.LogError("No se pudo encontrar el script 'Rikayon' en el objeto especificado.");
        }

        if (playerHealthObject != null)
        {
            playerHealth = playerHealthObject.GetComponent<PlayerHealth>();
        }

        // Verifica si se encontró el script PlayerHealth
        if (playerHealth == null)
        {
            Debug.LogError("No se pudo encontrar el script 'PlayerHealth' en el objeto especificado.");
        }

        // Inicializa los ojos con su color normal
        ChangeEyeColor(normalColor);
    }

    void Update()
    {
        // Comprobar si el jugador está dentro del rango del visionCollider
        if (visionCollider.bounds.Intersects(playerCollider.bounds))
        {
            isPlayerInRange = true; // El jugador está en rango
            float distance = Vector3.Distance(transform.position, playerCollider.transform.position);

            // Cambiar color de ojos basado en la proximidad dentro del visionCollider
            if (distance > visionCollider.bounds.extents.magnitude * 0.5f)
            {
                ChangeEyeColor(farColor); // Cambia a amarillo si está lejos
                movementScript.SetVelocity(slowSpeed);
            }
            else if (distance <= visionCollider.bounds.extents.magnitude * 0.5f)
            {
                ChangeEyeColor(closeColor); // Cambia a rojo si está más cerca
                movementScript.SetVelocity(fastSpeed);
            }

            // Mover hacia el jugador solo si está lo suficientemente cerca
            if (movementScript != null && distance <= visionCollider.bounds.extents.magnitude)
            {
                movementScript.MoveToPlayer(playerCollider.transform); // Mueve hacia el jugador

                movementScript.EndAttack(); // Termina la animación de ataque
            }

            // Si está lo suficientemente cerca, ataca
            if (Time.time >= nextAttackTime && distance <= visionCollider.bounds.extents.magnitude * 0.2f)
            {
                movementScript.StartAttack(); // Comienza la animación de ataque
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }

                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else
        {
            // Si el jugador está fuera del rango del visionCollider, vuelve a su color normal
            if (isPlayerInRange)
            {
                ChangeEyeColor(normalColor);
                isPlayerInRange = false; // El jugador ya no está en rango
                movementScript.EndAttack(); // Termina la animación de ataque
                movementScript.ResumePatrol(); // Reanuda la patrulla

                movementScript.SetVelocity(defaultSpeed);
            }

            
        }
    }

    // Cambia el color de los ojos del cangrejo
    private void ChangeEyeColor(Color color)
    {
        foreach (var eye in eyes)
        {
            Renderer renderer = eye.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color; // Cambia el color del material
            }
        }
    }
}
