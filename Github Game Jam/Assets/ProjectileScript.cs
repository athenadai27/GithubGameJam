using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float lerp;
    public Vector3 midPoint;
    public LayerMask playerMask;
    public LayerMask enemyMask;
    public Transform enemyTransform;
    public enum ProjectileStates { goingToPlayer, goingToEnemy };
    public ProjectileStates projectileState;
    public GameObject poof;
    public float lerpSpeed;
    public BoxCollider2D projectileCollider;
   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lerp += Time.deltaTime *lerpSpeed;
        Vector3 m1 = Vector3.Lerp(startPos, midPoint, lerp);
        Vector3 m2 = Vector3.Lerp(midPoint, endPos, lerp);
        Vector3 nextPosition = Vector3.Lerp(m1, m2, lerp);
        Vector3 vectorDif = nextPosition - transform.position;
        transform.up = vectorDif;
        Collider2D hitCollider;
        if(projectileState == ProjectileStates.goingToPlayer){
            hitCollider = Physics2D.OverlapBox(projectileCollider.bounds.center,projectileCollider.bounds.size,transform.eulerAngles.z,playerMask);
        } else{
            hitCollider = Physics2D.OverlapBox(projectileCollider.bounds.center,projectileCollider.bounds.size,transform.eulerAngles.z,enemyMask);
        }
        
        if (hitCollider)
        {
            switch (projectileState)
            {
                case ProjectileStates.goingToPlayer:
                    if (hitCollider.gameObject.CompareTag("Player"))
                    {
                        gameObject.SetActive(false);
                        hitCollider.gameObject.GetComponent<PlayerController>().Kill();
                    }
                    else if (hitCollider.CompareTag("HeldItem") && !hitCollider.gameObject.name.Contains("Sword"))
                    {
                        hitCollider.gameObject.GetComponent<ItemScript>().Break();
                        float additionalX = 0f;
                        if(transform.position.x > hitCollider.transform.position.x){
                            additionalX = 1f;
                        } else{
                            additionalX = -1f;
                        }
                        additionalX *= Random.Range(.25f,.75f)*(Vector3.Distance(startPos,endPos));
                        startPos = transform.position;
                        
                        endPos = new Vector3(startPos.x + additionalX, startPos.y-10f, 0f);
                        float oozeLerpHeight = Random.Range(3f, 5f);
                        midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * oozeLerpHeight;
                        lerp = 0f;

                    }
                    break;
                case ProjectileStates.goingToEnemy:
                    if(hitCollider.gameObject.GetComponentInChildren<EnemyHealth>()){
                        Debug.Log("hit");
                        hitCollider.gameObject.GetComponentInChildren<EnemyHealth>().Damage(2);
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
            gameObject.SetActive(false);
        }
    }

    public void WhackIntoEnemy()
    {
        startPos = transform.position;
        endPos = enemyTransform.position + Vector3.up;
        float oozeLerpHeight = Random.Range(3f, 7f);
        midPoint = startPos + (endPos - startPos) / 2 + Vector3.up * oozeLerpHeight;
        lerp = 0f;
        projectileState = ProjectileStates.goingToEnemy;
    }
}
