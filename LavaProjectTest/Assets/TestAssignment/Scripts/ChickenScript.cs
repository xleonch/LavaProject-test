using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    public Transform target;

    private Walker m_Walker;

    protected void Start()
    {
        m_Walker = GetComponent<Walker>();
    }

    void Update()
    {
        m_Walker.SetTarget(target.position);
    }
}