using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    Animator myAnim;
    [SerializeField]
    BoxCollider2D weaponCollider;
    [SerializeField]
    LayerMask enemyMask;

    public List<GameObject> hitEnemies;

    public float weaponDamage;
    public bool swinging;
    public ItemScript itemScript;

    public bool breaking;
    public bool freezeFrame;
    public float freezeTime;
    public GameObject weaponColliderObject;
    public EnemyHealth hitEnemy;
    public float damageAmount;
    public AudioSource swordAudio;
    public AudioClip swingClip;
    public AudioClip parryClip;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (freezeFrame)
        {
            if (Time.timeScale != 0f)
            {
                Time.timeScale = 0f;
            }
            if (Time.unscaledTime > freezeTime)
            {
                freezeFrame = false;
                Time.timeScale = 1f;
                if(hitEnemy){
                     hitEnemy.Damage(damageAmount);
                }
               
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1) && itemScript.itemState == ItemScript.ItemStates.held)
            {
                myAnim.SetTrigger("Swing");
                swordAudio.clip = swingClip;
                swordAudio.Play();
                 if(!swinging){
                    weaponColliderObject.SetActive(true);
                }
                swinging = true;
               
            }
            if (swinging && !breaking)
            {
                Collider2D enemyHitCheck = Physics2D.OverlapBox(weaponCollider.bounds.center, weaponCollider.bounds.size,weaponCollider.transform.eulerAngles.z, enemyMask);
                if (enemyHitCheck)
                {
                    if (!hitEnemies.Contains(enemyHitCheck.gameObject))
                    {
                        hitEnemies.Add(enemyHitCheck.gameObject);
                        float weaponModifier = 1f;
                        if (enemyHitCheck.gameObject.CompareTag("Tongue"))
                        {
                            enemyHitCheck.GetComponentInParent<FrogGruntAttackTest>().Retract();
                            hitEnemy = enemyHitCheck.GetComponentInParent<EnemyHealth>();
                            Parry();
                            weaponModifier = 2f;
                            damageAmount = weaponDamage*weaponModifier;
                            return;
                        }
                        else if (enemyHitCheck.GetComponent<FrogMiniboss>())
                        {
                            if (enemyHitCheck.GetComponent<FrogMiniboss>().bossState == FrogMiniboss.BossStates.leapDown)
                            {
                                enemyHitCheck.GetComponent<FrogMiniboss>().ItemHit();
                                Parry();
                                weaponModifier = 2f;
                            }
                        }
                        else if (enemyHitCheck.GetComponent<FrogKingScript>())
                        {
                            if (enemyHitCheck.GetComponent<FrogKingScript>().bossState == FrogKingScript.BossStates.leapDown)
                            {
                                enemyHitCheck.GetComponent<FrogKingScript>().ItemHit();
                                Parry();
                                weaponModifier = 2f;
                            }
                        }
                        else if (enemyHitCheck.GetComponent<ToxicSlimeProjectile>())
                        {
                            enemyHitCheck.GetComponent<ToxicSlimeProjectile>().WhackIntoEnemy();
                            Parry();
                        }else if(enemyHitCheck.GetComponent<ProjectileScript>()){
                            enemyHitCheck.GetComponent<ProjectileScript>().WhackIntoEnemy();
                            Parry();
                        }
                        else if (enemyHitCheck.GetComponent<SoundWave>())
                        {
                            enemyHitCheck.GetComponent<SoundWave>().Reverse();
                            Parry();
                            // enemyHitCheck.transform.right = enemyHitCheck.transform.right*-1f;
                            // enemyHitCheck.gameObject.SetActive(false);
                        }
                        if (enemyHitCheck.gameObject.GetComponentInParent<EnemyHealth>())
                        {
                            enemyHitCheck.gameObject.GetComponentInParent<EnemyHealth>().Damage(weaponDamage * weaponModifier);
                        }

                    }

                    breaking = true;

                }
            }
        }

    }

    public void DoneSwinging()
    {
        weaponColliderObject.SetActive(false);
        if (breaking)
        {
            itemScript.Break();
        }
        freezeFrame = false;
        Time.timeScale = 1f;
        swinging = false;
        hitEnemies.Clear();
    }

    public void Parry()
    {
        swordAudio.clip = parryClip;
        swordAudio.Play();
        freezeFrame = true;
        freezeTime = Time.unscaledTime + .5f;
    }
}
