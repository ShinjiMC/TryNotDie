using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public List<GameObject> healableObjects; // Lista de objetos que pueden ser curados
    public BoxCollider lavaCollider; // Collider de lava
    public float damageAmount = 10f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de que la lava tenga un BoxCollider
        if (lavaCollider == null)
        {
            lavaCollider = GetComponent<BoxCollider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra en el collider es uno de los objetos curables
        foreach (GameObject healable in healableObjects)
        {
            if (other.gameObject == healable)
            {
                // Obtener el componente de salud y llamar al método de curación
                EnemyHealth healthComponent = healable.GetComponent<EnemyHealth>();
                if (healthComponent != null)
                {
                    healthComponent.AddHealth(10f); // Llamar a la función de curar
                    Debug.Log($"{healable.name} ha sido curado.");
                }
                break; // Salir del bucle si se encuentra el objeto
            }
        }

        if (player  != null)
        {
            PlayerHealth playerHealthComponent = other.GetComponent<PlayerHealth>();
            if (playerHealthComponent != null)
            {
                playerHealthComponent.TakeDamage(damageAmount); // Llamar al método de daño
                Debug.Log($"{other.gameObject.name} ha recibido {damageAmount} de daño.");
            }
        }
    }
}

