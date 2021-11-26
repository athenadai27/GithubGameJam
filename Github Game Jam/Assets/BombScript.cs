using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public LayerMask explosionMask;
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
        if (!exploding)
        {
            Collider2D bombHit = Physics2D.OverlapBox(bombCollider.bounds.center, bombCollider.bounds.size, 0, explosionMask);
            if (bombHit || Time.time > lifeTime)
            {
                myAnim.SetTrigger("Explode");
                exploding = true;
                Collider2D explosionHit = Physics2D.OverlapCircle(explosionCollider.bounds.center, explosionCollider.radius, explosionMask);
            }
        }


    }
}
