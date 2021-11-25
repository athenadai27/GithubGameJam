using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    Animator myAnim;
    [SerializeField]
    CircleCollider2D weaponCollider;
    [SerializeField]
    LayerMask enemyMask;

    public List<GameObject> hitEnemies;

    public float weaponDamage;
    public bool swinging;
    public ItemScript itemScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            myAnim.SetTrigger("Swing");
            swinging = true;
        }
        if (swinging)
        {
            Collider2D enemyHitCheck = Physics2D.OverlapCircle(weaponCollider.bounds.center, weaponCollider.radius, enemyMask);
            if (enemyHitCheck)
            {
                if (!hitEnemies.Contains(enemyHitCheck.gameObject))
                {
                    hitEnemies.Add(enemyHitCheck.gameObject);
                    float weaponModifier = 1f;
                    if (enemyHitCheck.gameObject.CompareTag("Tongue"))
                    {
                        enemyHitCheck.GetComponentInParent<FrogGruntAttackTest>().Retract();
                        weaponModifier = 2f;
                    } else if(enemyHitCheck.GetComponent<FrogMiniboss>()){
                        if (enemyHitCheck.GetComponent<FrogMiniboss>().bossState == FrogMiniboss.BossStates.leapDown)
                        {
                            enemyHitCheck.GetComponent<FrogMiniboss>().ItemHit();
                            weaponModifier = 2f;
                        }
                    } else if(enemyHitCheck.GetComponent<ToxicSlimeProjectile>()){
                        enemyHitCheck.GetComponent<ToxicSlimeProjectile>().WhackIntoEnemy();
                    }else if(enemyHitCheck.GetComponent<SoundWave>()){
                        enemyHitCheck.GetComponent<SoundWave>().Reverse();
                       // enemyHitCheck.transform.right = enemyHitCheck.transform.right*-1f;
                       // enemyHitCheck.gameObject.SetActive(false);
                    }
                    if(enemyHitCheck.gameObject.GetComponentInParent<EnemyHealth>()){
                        enemyHitCheck.gameObject.GetComponentInParent<EnemyHealth>().Damage(weaponDamage*weaponModifier);
                    }
                    
                }

                itemScript.Break();

            }
        }
    }

    public void DoneSwinging()
    {
        swinging = false;
        hitEnemies.Clear();
    }
}
