using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float playerMaxHealth = 100f;
    [SerializeField] public float playerCurrentHealth;

    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage (float amout)
    {
        playerCurrentHealth -= amout;

        if (playerCurrentHealth <= 0)
        {
            animator.SetTrigger("IsDead");
            Debug.Log("cc");
        }
    }
}
