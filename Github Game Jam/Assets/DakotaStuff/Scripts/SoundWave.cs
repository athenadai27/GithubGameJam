using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public float moveSpeed;
    public BoxCollider2D boxCollider;
    public LayerMask hitMask;
    public LayerMask enemyMask;
    public bool goingToEnemy;
 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 travelVector = transform.right * moveSpeed * Time.deltaTime;
        RaycastHit2D boomHit;
        if(goingToEnemy){
            boomHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, travelVector, travelVector.magnitude, enemyMask);
        } else{
          boomHit  = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, travelVector, travelVector.magnitude, hitMask);
        }
        if (boomHit)
        {
            if (boomHit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log(gameObject.name);
                 gameObject.SetActive(false);
                boomHit.collider.gameObject.GetComponent<PlayerController>().Kill();
                
            }
            else if (boomHit.collider.CompareTag("HeldItem") && !boomHit.collider.gameObject.name.Contains("Sword"))
            {
                boomHit.collider.gameObject.GetComponent<ItemScript>().Break();

                gameObject.SetActive(false);

            } else if(boomHit.collider.GetComponentInParent<EnemyHealth>()){
              boomHit.collider.GetComponentInParent<EnemyHealth>().Damage(2f);  
               gameObject.SetActive(false);
               goingToEnemy = false;
            }
        }
        else
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }

    }

    public void Reverse(){
        transform.right = transform.right*-1f;
        goingToEnemy = true;
    }


}
