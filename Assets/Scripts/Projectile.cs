using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject shooter; // So it can fire only at the player
    float damageCaused; // damage caused, can be overwritten by shooter

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
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

    void OnCollisionEnter(Collision collision)
    {
        var layerCollidedWith = collision.gameObject.layer;
        if (layerCollidedWith != shooter.layer)
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
