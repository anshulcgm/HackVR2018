using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] float maxHealthPoints = 100f;
    public float currentHealthPoints;

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0) { Destroy(gameObject); }
    }
    // Use this for initialization
    void Start () {
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
