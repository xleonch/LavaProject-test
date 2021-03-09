using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    public string animationFlag;
    public WalkerConfig walkerConfig;

    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private Transform m_Visual;
    private Vector3 m_Heading;
    private bool m_IsWalking;

    public void SetTarget(Vector3 position)
    {
        if (m_Agent.enabled)
        {
            m_Agent.SetDestination(position);
        }
    }

    protected void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Visual = transform.GetChild(0);
        m_Animator = m_Visual.GetComponent<Animator>();
        m_Agent.speed = walkerConfig.walkerSpeed;
    }

    protected void Update()
    {
        if (m_Agent.velocity.magnitude > Mathf.Epsilon)
        {
            m_Heading = m_Agent.velocity;
            if (!m_IsWalking)
            {
                m_IsWalking = true;
                m_Animator.SetBool(animationFlag, true);
            }
        }
        else
        {
            if (m_IsWalking)
            {
                m_IsWalking = false;
                m_Animator.SetBool(animationFlag, false);
            }
        }

        //if (physics.raycast(transform.position + vector3.up, vector3.down, out var hit, 2, 1 << 8))
        //{
        //    transform.rotation = quaternion.fromtorotation(transform.forward, m_heading) * transform.rotation;
        //    transform.rotation = quaternion.fromtorotation(transform.up, hit.normal) * transform.rotation;
        //    m_visual.localposition = -vector3.up * (hit.distance - 1);
        //}
    }
}
