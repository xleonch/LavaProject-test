using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Rigidbody prefabProjectile;
    public ProjectileConfig projectileConfig;

    public TwoBoneIKConstraint ikAim;
    public Transform aimTarget;
    public Transform rArmPalm;

    private Camera m_Camera;
    private Walker m_Walker;
    private bool m_IsShooting;
    private bool m_IsShootingMode;
    private bool m_ShootingInProcess;

    protected void Start()
    {
        m_Camera = Camera.main;
        m_Walker = GetComponent<Walker>();
    }

    protected void Update()
    {
        if (m_IsShootingMode)
        {
            if (m_IsShooting)
            {
                if (Input.GetMouseButtonUp(0))
                    m_IsShooting = false;
            }
            else
            {
                if (!m_ShootingInProcess && Input.GetMouseButtonDown(0))
                {
                    m_IsShooting = true;
                    StartCoroutine(MonsterShooting());
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    m_Walker.SetTarget(hit.point);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        m_IsShootingMode = true;
    }

    public Vector3 AimHand()
    {
        var ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit))
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (!plane.Raycast(ray, out var enter))
            {
                return aimTarget.position;
            }
            hit = new RaycastHit() { point = ray.GetPoint(enter) };
        }
        aimTarget.position = hit.point;
        var lookDirection = hit.point - transform.position;
        lookDirection.y = transform.forward.y;
        transform.rotation = Quaternion.FromToRotation(transform.forward, lookDirection) * transform.rotation;
        return aimTarget.position;
    }

    public IEnumerator MonsterShooting()
    {
        m_ShootingInProcess = true;
           var aimMoment = Time.time + 0.3f;
        var delta = 0f;
        // HandWeight Up
        do
        {
            AimHand();
            delta = (Time.time - aimMoment) / 0.3f;
            delta = Mathf.Clamp01(delta);
            ikAim.weight = delta;
            yield return null;
        } while (delta < 1);

        // Shooting
        do
        {
            var origin = rArmPalm.position;
            var projectile = Instantiate(prefabProjectile, origin, transform.rotation);
            projectile.AddForce((AimHand() - origin).normalized * projectileConfig.projectileVelocity, ForceMode.VelocityChange);
            Destroy(projectile.gameObject, 5);
            var waitUntil = Time.time + 0.5f;
            while (waitUntil > Time.time)
            {
                AimHand();
                yield return null;
            }
        } while (m_IsShooting);

        // HandWeight Down
        aimMoment = Time.time + 0.3f;
        do
        {
            delta = 1.0f - ((Time.time - aimMoment) / 0.5f);
            delta = Mathf.Clamp01(delta);
            ikAim.weight = delta;
            yield return null;
        } while (delta > 0);
        m_ShootingInProcess = false;
    }
}