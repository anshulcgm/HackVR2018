using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // The way the code works is that the shooter and target must be on seperate layers. Additionally the player must be tagged with the player tag.
    [Tooltip("The attack radius of this game object, in units")]
    [SerializeField] float attackRadius = 4f;
    [Tooltip("The damage this game object's projectiles deal. This overwrites the projectiles damage, unless it is set to 0.")]
    [SerializeField] float damagePerShot = 9f;
    [Tooltip("Attack Rate, in seconds")]
    [SerializeField] float secondsBetweenShots = 0.5f;
    [Tooltip("The projectile to spawn")]
    [SerializeField] GameObject projectileToUse;
    [Tooltip("The location to spawn the projectile.")]
    [SerializeField] GameObject projectileSocket; // The place to spawn the projectile

    bool isAttacking = false;
    float currentHealthPoints;
    GameObject player = null;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("FireProjectile", 0f, secondsBetweenShots);
        }
        
        if (distanceToPlayer > attackRadius)
        {
            isAttacking = false;
            CancelInvoke();
        }
    }

    void FireProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);
        projectileComponent.SetShooter(gameObject);
        Debug.Log("shooting");
        projectileComponent.targetPos = player.transform.position;
        projectileComponent.target = player;
    }

    void OnDrawGizmos()
    {
        // Draw attack sphere 
        Gizmos.color = new Color(255f, 0, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
