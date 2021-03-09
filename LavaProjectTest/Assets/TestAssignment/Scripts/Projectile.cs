using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    public ParticleSystem ParticleExplosion;
    private ParticleSystem m_ParticleSystem;
    private Rigidbody m_Rigidbody;
    
    protected void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        var collisionPoint = collision.GetContact(0).point;
        var rb = collision.rigidbody;
        if (rb != null)
        {
            var animator = rb.GetComponentInParent<Animator>();
            if (animator != null)
                animator.enabled = false;
            var agent = rb.GetComponentInParent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = false;
            rb.AddForceAtPosition(m_Rigidbody.velocity, collisionPoint, ForceMode.VelocityChange);
        }
        var em = m_ParticleSystem.emission;
        em.enabled = false;
        GetComponent<Collider>().enabled = false;
        Instantiate(ParticleExplosion, collisionPoint, Quaternion.identity);
    }
}
