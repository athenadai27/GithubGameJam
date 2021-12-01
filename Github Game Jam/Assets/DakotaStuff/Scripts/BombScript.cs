using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public LayerMask explosionMask;
    public LayerMask bombMask;
    public CircleCollider2D explosionCollider;
    public BoxCollider2D bombCollider;
    public float lifeTime;
    public Animator myAnim;
    public bool exploding;
    // Start is called before the first frame update
    void OnEnable()
    {
        lifeTime = Time.time + 5f;
    }

    // Update is called once per frame
    void Update()
    {
       
            Collider2D bombHit = Physics2D.OverlapBox(bombCollider.bounds.center, bombCollider.bounds.size, 0, bombMask);
            if (exploding)
            {
                Collider2D explosionHit = Physics2D.OverlapCircle(explosionCollider.bounds.center, explosionCollider.radius, explosionMask);
                if (explosionHit)
                {
                    if(explosionHit.gameObject.GetComponent<CheckForBombHit>() != null){
                        explosionHit.gameObject.GetComponent<CheckForBombHit>().BombHit();
                    }
                    else if (explosionHit.gameObject.CompareTag("Player"))
                    {

                    }else if(explosionHit.gameObject.GetComponentInChildren<Bombproof>()){
                       
                    }
                    else if (explosionHit.gameObject.GetComponentInParent<EnemyHealth>())
                    {
                        explosionHit.gameObject.GetComponentInParent<EnemyHealth>().Damage(3f);
                        if (explosionHit.gameObject.GetComponentInParent<FrogKingScript>())
                        {
                            if (explosionHit.gameObject.GetComponentInParent<FrogKingScript>().bossState == FrogKingScript.BossStates.leapDown)
                            {
                                explosionHit.gameObject.GetComponentInParent<FrogKingScript>().ItemHit();
                            }
                        }
                    }
                }
            }
            else
            {
                if (bombHit || Time.time > lifeTime)
                {
                    myAnim.SetTrigger("Explode");
                    exploding = true;

                }
            }

        


    }
}
