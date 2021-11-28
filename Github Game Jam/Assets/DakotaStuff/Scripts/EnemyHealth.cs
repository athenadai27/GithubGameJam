using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public SpriteRenderer enemySprite;
    public float enemyDamageTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > enemyDamageTime){
            enemySprite.color = Color.white;
        }
    }

    public void Damage(float damageAmount){
        currentHealth -= damageAmount;
        enemySprite.color = Color.red;
        enemyDamageTime = Time.time + .5f;
    }
}
