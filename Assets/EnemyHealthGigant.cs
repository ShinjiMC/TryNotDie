using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class EnemyHealthGigant : MonoBehaviour
{
    public float health = 100f;
    public Renderer enemyRenderer;
    private Color originalColor;
    public float damageEffectDuration = 0.2f;

    // Nuevo par�metro p�blico para el objeto que aparecer� al morir
    public GameObject objectOnDeath;

    private void Start()
    {
        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;

        // Asegurarse de que el objeto que aparecer� al morir est� inicialmente desactivado
        if (objectOnDeath != null)
        {
            objectOnDeath.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            DestroyObject ball = collision.gameObject.GetComponent<DestroyObject>();
            if (ball != null)
            {
                TakeDamage(ball.damage);
                Destroy(collision.gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (enemyRenderer != null)
            StartCoroutine(ChangeColorOnDamage());
        if (health <= 0)
            Die();
    }

    public void AddHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0f, 100f); // Cambia 100f al m�ximo que desees
        Debug.Log($"Salud actual: {health}");
    }

    void Die()
    {
        // Si hay un objeto para aparecer al morir
        if (objectOnDeath != null)
        {
            // Activarlo y moverlo a la posici�n del enemigo
            objectOnDeath.transform.position = transform.position;
            objectOnDeath.SetActive(true);
        }

        Destroy(gameObject);
    }

    IEnumerator ChangeColorOnDamage()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(damageEffectDuration);
        enemyRenderer.material.color = originalColor;
    }
}
