using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Necesario para usar NavMesh

public class Rikayon : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform[] waypoints; // Los waypoints en el terreno
    private int currentWaypoint = 0;
    private bool isPatrolling = true; // Controla si el enemigo está patrullando
    private bool isAttacking = false;

    void Start()
    {
        // Setea el primer destino al primer waypoint
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void Update()
    {
        if (isPatrolling)
        {
            // Verifica si el enemigo ha llegado al waypoint actual
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextWaypoint();
            }
        }
        if(isAttacking)
        {
            animator.SetTrigger("Attack_1");
        }

        // Controla las animaciones de caminar
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("isWalking", true);  // Activa la animación de caminar
        }
        else
        {
            animator.SetBool("isWalking", false);  // Detiene la animación de caminar
        }
    }

    // Mueve el agente al siguiente waypoint en la lista
    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        // Cambia al siguiente waypoint en el array (de manera cíclica)
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    // Función para mover al enemigo hacia el jugador
    public void MoveToPlayer(Transform player)
    {
        if (isAttacking)
            return;
        isPatrolling = false; // Detiene el patrullaje
        agent.SetDestination(player.position); // El enemigo va hacia el jugador
    }

    // Función para reanudar la patrulla entre waypoints
    public void ResumePatrol()
    {
        if (isAttacking)
            return;
        isPatrolling = true; // Reactiva el patrullaje
        GoToNextWaypoint(); // Continúa con el siguiente waypoint
    }

    public void StartAttack()
    {
        isAttacking = true; // Detiene el movimiento durante el ataque
        agent.isStopped = true; // Detiene el NavMeshAgent para no moverse
    }
    public void EndAttack()
    {
        isAttacking = false; // Permite volver a moverse
        agent.isStopped = false; // Reactiva el NavMeshAgent
    }

    public void SetVelocity(float value)
    {
        agent.isStopped = true;
        agent.speed = value;
        agent.isStopped = false;
    }
}