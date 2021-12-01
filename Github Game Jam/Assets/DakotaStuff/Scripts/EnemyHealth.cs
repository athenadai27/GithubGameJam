using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public SpriteRenderer enemySprite;
    public float enemyDamageTime;
    public GameObject poof;
    public GameObject parentObject;
    public Transform canvasTransform;
    public Vector3 originalScale;
    public Vector3 originalPosition;
    public Animator enemyAnim;
    public bool boss;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = parentObject.transform.localScale;
        originalPosition = parentObject.transform.position;
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
        if(currentHealth <= 0 && !boss){
            Kill();
        }
    }

    public virtual void Reset(){
       currentHealth = maxHealth;
       parentObject.transform.position = originalPosition;
       parentObject.transform.localScale = originalScale;
       canvasTransform.localScale = originalScale;
       enemySprite.color = Color.white;
       enemyAnim.Rebind();
    }

    public virtual void Kill(){
        Instantiate(poof,transform.position,poof.transform.rotation);
        parentObject.SetActive(false);
    }
}
