using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public Renderer enemyRenderer;
    private Color originalColor;
    public float damageEffectDuration = 0.2f;

    private void Start()
    {
        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;
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
        health = Mathf.Clamp(health, 0f, 100f); // Cambia 100f al máximo que desees
        Debug.Log($"Salud actual: {health}");
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator ChangeColorOnDamage()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(damageEffectDuration);
        enemyRenderer.material.color = originalColor;
    }
}
