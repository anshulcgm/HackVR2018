using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [Tooltip("How high the arc should be, in units")]
    [SerializeField] int arcHeight = 1;
    [Tooltip("Speed of the projectile")]
    [SerializeField] float projectileSpeed = 10;
    public GameObject target;
    [Tooltip("The Game Object that shot the projectile. This is set in code, adn thus does not to be initialized in the inspector.")]
    [SerializeField] GameObject shooter;
    public Vector3 targetPos;

    Vector3 startPos;
    [Tooltip("Damage the projectile deals. Overwritten by the shooter's damage, unless it is 0.")]
    [SerializeField] float damageCaused;

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Compute the next position, with arc added in
        float x0 = startPos.x;
        float x1 = targetPos.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, projectileSpeed * Time.deltaTime);
        float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
        float baseZ = Mathf.Lerp(startPos.z, targetPos.z, (nextX - x0) / dist);

        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, baseZ);

        // Rotate to face the next position, and then move there
        transform.rotation = Quaternion.LookRotation(nextPos - transform.position);
        transform.position = nextPos;
    }

    public void SetDamage(float damage)
    {
        if(damage != 0)
            damageCaused = damage;
    }

    public float GetDefaultLaunchSpeed()
    {
        return projectileSpeed;
    }
    // Compare the tag of the shooter and the object the projectile collided with, if they are different, try to     deal damage.
    void OnCollisionEnter(Collision collision)
    {
        var tagCollidedWith = collision.gameObject.tag;
        if (tagCollidedWith != shooter.tag)
        {
            DamageIfDamageable(collision);
            Destroy(gameObject);
        }
    }

    private void DamageIfDamageable(Collision collision)
    {
        collision.gameObject.GetComponent<Health>().TakeDamage(damageCaused);
        Debug.Log(collision.gameObject.GetComponent<Health>().currentHealthPoints);
    }
}
