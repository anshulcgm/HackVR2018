using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Adds health to the game object this script is attached to. If the health is reduced to 0, the game object will be destroyed.
 */
public class Health : MonoBehaviour {
    [Tooltip("Maximum Health Points this entity has.")]
    [SerializeField] float maxHealthPoints = 100f;
    [Tooltip("The current health of the entity. If this value is set to 0 in the inspector, the script will automatically set the current health to the max healthh.")]
    public float currentHealthPoints = 0f;

    // Deals damage to the owner of this script, ensures that health is never at 0 or above the maximum health.
    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints == 0) { Destroy(gameObject); }
    }
    void Awake () {
        if(currentHealthPoints == 0)
            currentHealthPoints = maxHealthPoints;
    }
    void Update()
    {
        if (currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
