using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// Apply forward force to instantiated prefab
    /// </summary>
    public class LaunchProjectile : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The projectile that's created")]
        GameObject m_ProjectilePrefab = null;

        [SerializeField]
        [Tooltip("The point that the project is created")]
        Transform m_StartPoint = null;

        [SerializeField]
        [Tooltip("The speed at which the projectile is launched")]
        float m_LaunchSpeed = 1.0f;

        [SerializeField]
        [Tooltip("Max number of shots before overheating in a short period")]
        int maxShotsBeforeOverheat = 5;

        [SerializeField]
        [Tooltip("Time required to cool down after overheating (in seconds)")]
        float cooldownTime = 5.0f;

        [SerializeField]
        [Tooltip("Time window in seconds for rapid shots to count toward overheating")]
        float shotInterval = 1.0f;

        [SerializeField]
        [Tooltip("Material to change color during overheat (must use URP Lit shader)")]
        Material weaponMaterial;

        [SerializeField]
        [Tooltip("Color when the weapon is overheated")]
        Color overheatColor = Color.red;

        [SerializeField]
        [Tooltip("Original color of the weapon")]
        Color normalColor = Color.white;

        private int shotsFired = 0;  // Número de disparos dentro del tiempo permitido
        private bool isOverheated = false;  // Si el arma está sobrecalentada
        private float lastShotTime = 0f;  // Tiempo del último disparo
        private bool isHeld = false;
        XRGrabInteractable grabInteractable;
        void Awake()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            isHeld = true;
        }

        private void OnSelectExited(SelectExitEventArgs args)
        {
            isHeld = false;
        }

        void Update()
        {
            // Enfriar el arma gradualmente si no está sobrecalentada y se disparó hace tiempo
            if (!isOverheated && Time.time - lastShotTime > shotInterval)
            {
                GraduallyCoolDown();
            }
        }

        public void Fire()
        {
            // Evitar disparar si está sobrecalentado
            if (!isHeld || isOverheated)
            {
                Debug.Log("Weapon is not held or is overheated.");
                return;
            }

            GameObject newObject = Instantiate(m_ProjectilePrefab, m_StartPoint.position, m_StartPoint.rotation, null);

            if (newObject.TryGetComponent(out Rigidbody rigidBody))
                ApplyForce(rigidBody);

            // Reproducir sonido al lanzar el proyectil
            PlayLaunchSound(newObject);

            // Verificar si los disparos son en rápida sucesión para contar hacia el sobrecalentamiento
            HandleRapidShots();
        }

        void ApplyForce(Rigidbody rigidBody)
        {
            Vector3 force = m_StartPoint.forward * m_LaunchSpeed;
            rigidBody.AddForce(force);
        }

        void PlayLaunchSound(GameObject projectile)
        {
            // Intenta obtener el AudioSource del proyectil
            if (projectile.TryGetComponent(out AudioSource audioSource))
            {
                audioSource.Play(); // Reproduce el sonido
            }
        }

        // Verifica si los disparos fueron en rápida sucesión
        void HandleRapidShots()
        {
            float currentTime = Time.time;

            // Si el tiempo entre disparos es menor o igual al intervalo permitido, cuenta el disparo
            if (currentTime - lastShotTime <= shotInterval)
            {
                shotsFired++;
            }
            else
            {
                // Si el tiempo excede el intervalo, reinicia el contador de disparos rápidos
                shotsFired = 1;
            }

            // Actualiza el tiempo del último disparo
            lastShotTime = currentTime;

            // Si se ha alcanzado el número máximo de disparos rápidos, sobrecalentar el arma
            if (shotsFired >= maxShotsBeforeOverheat)
            {
                StartCoroutine(OverheatWeapon());
            }
            else
            {
                // Si no está sobrecalentado, mantener el color normal
                ChangeWeaponColor(normalColor);
            }
        }

        // Maneja el enfriamiento gradual si no hay disparos en un tiempo
        void GraduallyCoolDown()
        {
            if (shotsFired > 0)
            {
                // Reducir gradualmente el contador de disparos
                shotsFired = Mathf.Max(0, shotsFired - 1);
            }
        }

        // Coroutine para manejar el sobrecalentamiento
        IEnumerator OverheatWeapon()
        {
            // Marcar el arma como sobrecalentada y cambiar el color a rojo
            isOverheated = true;
            ChangeWeaponColor(overheatColor); // Cambia a rojo
            Debug.Log("Weapon overheated!");

            // Esperar el tiempo de enfriamiento
            yield return new WaitForSeconds(cooldownTime);

            // Enfriar el arma, reiniciar el contador de disparos y volver al color normal
            shotsFired = 0;
            isOverheated = false;
            ChangeWeaponColor(normalColor); // Vuelve al color blanco
            Debug.Log("Weapon cooled down.");
        }

        // Función para cambiar el color del material del arma
        void ChangeWeaponColor(Color color)
        {
            if (weaponMaterial != null)
            {
                weaponMaterial.SetColor("_BaseColor", color); // Cambiar el color sin transición
            }
            else
            {
                Debug.LogWarning("Weapon material is not assigned!");
            }
        }
    }
}
