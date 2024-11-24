namespace UnityEngine.XR.Content.Interaction
{
    
    public class DestroyObject : MonoBehaviour
    {
        public float damage = 10f;
        [SerializeField]
        [Tooltip("Time before destroying in seconds.")]
        float m_Lifetime = 5f;

        void Start()
        {
            Destroy(gameObject, m_Lifetime);
        }
    }
}
