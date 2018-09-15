using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [Tooltip("Maximum Health Points this entity has.")]
    [SerializeField] float maxHealthPoints = 100f;
    [Tooltip("The current health of the entity. If this value is set to 0 in the inspector, the script will automatically set the current health to the max healthh.")]
    public float currentHealthPoints = 0f;

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0) { Destroy(gameObject); }
    }
    // Use this for initialization
    void Start () {
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
