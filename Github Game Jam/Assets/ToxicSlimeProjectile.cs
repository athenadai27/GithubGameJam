using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSlimeProjectile : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float lerp;
    public Vector3 midPoint;
    public LayerMask playerMask;
    public LayerMask enemyMask;
    public Transform bossTransform;
    public enum DiceStates { goingToPlayer, goingToEnemy };
    public DiceStates diceState;
    public GameObject poof;
    public bool isToxic;
    public GameObject toxicPuddle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lerp += Time.deltaTime / 2f;
        Vector3 m1 = Vector3.Lerp(startPos, midPoint, lerp);
        Vector3 m2 = Vector3.Lerp(midPoint, endPos, lerp);
        Vector3 nextPosition = Vector3.Lerp(m1, m2, lerp);
        Vector3 vectorDif = nextPosition - transform.position;
        transform.up = vectorDif;
        RaycastHit2D hitRay;
        if(diceState == DiceStates.goingToPlayer){
            hitRay = Physics2D.Raycast(transform.position, vectorDif, vectorDif.magnitude, playerMask);
        } else{
            hitRay = Physics2D.Raycast(transform.position, vectorDif, vectorDif.magnitude, enemyMask);
        }
        
        if (hitRay)
        {
            switch (diceState)
            {
                case DiceStates.goingToPlayer:
                    if (hitRay.collider.gameObject.CompareTag("Player"))
                    {
                        gameObject.SetActive(false);
                        hitRay.collider.gameObject.GetComponent<PlayerController>().Kill();
                    }
                    else if (hitRay.collider.CompareTag("HeldItem") && !hitRay.collider.gameObject.name.Contains("Sword"))
                    {
                        hitRay.collider.gameObject.GetComponent<ItemScript>().Break();
                        
                        startPos = transform.position;
                        endPos = new Vector3(startPos.x + Random.Range(-5f, 5f), -2.75f, 0f);
                        float oozeLerpHeight = Random.Range(3f, 5f);
                        midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * oozeLerpHeight;
                        lerp = 0f;

                    }
                    break;
                case DiceStates.goingToEnemy:
                    if(hitRay.collider.gameObject.GetComponent<EnemyHealth>()){
                        hitRay.collider.gameObject.GetComponent<EnemyHealth>().Damage(2);
                        Instantiate(poof,transform.position,poof.transform.rotation);
                        gameObject.SetActive(false);
                    }
                    break;
            }

        }
        else
        {
            transform.position = nextPosition;
        }




        if (transform.position == m2)
        {
            Instantiate(poof,transform.position,poof.transform.rotation);
            if(isToxic){
                Instantiate(toxicPuddle,transform.position + Vector3.down*.5f,toxicPuddle.transform.rotation);
            } 
            
            gameObject.SetActive(false);
        }
    }

    public void WhackIntoEnemy()
    {
        startPos = transform.position;
        endPos = bossTransform.position + Vector3.up;
        float oozeLerpHeight = Random.Range(3f, 7f);
        midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * oozeLerpHeight;
        lerp = 0f;
        diceState = DiceStates.goingToEnemy;
    }
}
