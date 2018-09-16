using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Logic for a turret esc enemy that will shoot at the player if it is within its attack range.
 */
public class Enemy : MonoBehaviour {
    // NOTE: The way the code works is that the shooter and target MUST have seperate tags, the player has to be tagged with the player tag.
    [Tooltip("The attack radius of this game object, in units")]
    [SerializeField] float attackRadius = 4f;
    [Tooltip("The damage this game object's projectiles deal. This overwrites the projectiles damage, unless it is set to 0.")]
    [SerializeField] float damagePerShot = 9f;
    [Tooltip("Attack Rate, in seconds")]
    [SerializeField] float secondsBetweenShots = 0.5f;
    [Tooltip("The projectile to spawn")]
    [SerializeField] GameObject projectileToUse;
    [Tooltip("The location to spawn the projectile.")]
    [SerializeField] GameObject projectileSocket;
    [Tooltip("How fast the object rotates")]
    [SerializeField] float RotaionSpeed =20f;
    bool isAttacking = false;
    float currentHealthPoints;
    GameObject player = null;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Rotates the enemy to face the player, rotate only along the Y axis.
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotaionSpeed);
        }
    }
    void Update()
    {
        try
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                // Repeatedly calls the function every (secondBetweenShots) seconds. Only stops with a CancelInvoke call.
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShots);
            }

            if ((distanceToPlayer > attackRadius))
            {
                isAttacking = false;
                CancelInvoke();
            }
        }
        catch
        {
            CancelInvoke();
        }
    }
    // Code to fire a projectile, this logic both instantiates and sets up the projectile so that it recoginizes both its target, and shooter.
    void FireProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);
        projectileComponent.SetShooter(gameObject);
        projectileComponent.targetPos = player.transform.position;
        projectileComponent.target = player;
    }
    // Draws gizmos to help visualize the attack radius.
    void OnDrawGizmos()
    {
        // Draw attack sphere 
        Gizmos.color = new Color(255f, 0, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
