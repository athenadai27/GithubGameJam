using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogGruntAttackTest : MonoBehaviour
{
    public LineRenderer tongue;
    public Transform target;
    public Animator myAnim;
    public Vector3 startPos;
    public Vector3 endPos;
    public float lerp;
    public float tongueSpeed;
    public float distanceFromEndStart;
    public float outDistanceFromEndStart;
    public LayerMask playerMask;
    public bool capturedPlayer;
    public Transform playerTransform;
    public Vector3 targetPos;

    public enum TongueStates { goingOut, goingIn, dormant };
    public TongueStates tongueState = TongueStates.dormant;

    public EnemyAlert enemyAlert;
    public bool attacking;

    public EdgeCollider2D edgeCollider;
    // Start is called before the first frame update
    void Start()
    {
        endPos = target.position;
        distanceFromEndStart = Vector3.Distance(endPos, tongue.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            myAnim.SetTrigger("Attack");
        }
        switch (tongueState)
        {
            case TongueStates.dormant:
                break;
            case TongueStates.goingIn:
                lerp += Time.deltaTime * tongueSpeed*2f;
                targetPos += (tongue.transform.position - targetPos).normalized*tongueSpeed*Time.deltaTime;
               // targetPos = Vector3.Lerp(endPos, tongue.transform.position, lerp);
                if(capturedPlayer){
                    playerTransform.position = targetPos;
                }
                float distanceToTarget = Vector3.Distance(tongue.transform.position, targetPos);
                float segmentDistance = distanceToTarget / tongue.positionCount;
                Vector3 targetDirection = (targetPos - tongue.transform.position).normalized;
                float distanceFromEnd = Vector3.Distance(targetPos, endPos);
                for (int i = 0; i < tongue.positionCount; i++)
                {
                    
                    tongue.SetPosition(i, tongue.transform.position + targetDirection * (i * segmentDistance) + Vector3.up * Mathf.Sin(i * segmentDistance) * Mathf.Sin(1 - (distanceFromEndStart - distanceFromEnd) / distanceFromEndStart));
                }
                if (distanceFromEnd >= Vector3.Distance(tongue.transform.position,endPos))
                {
                    tongue.gameObject.SetActive(false);
                    if(capturedPlayer){
                        playerTransform.GetComponent<PlayerController>().Release();
                        capturedPlayer = false;
                    }
                    if(enemyAlert){
                        enemyAlert.alertLevel = EnemyAlert.AlertLevels.pursuit;
                    }
                    
                    tongueState = TongueStates.dormant;
                    attacking = false;
                }
                break;
            case TongueStates.goingOut:
                lerp += Time.deltaTime * tongueSpeed;
                targetPos += (endPos - targetPos).normalized*tongueSpeed*Time.deltaTime;
                //targetPos = Vector3.Lerp(tongue.transform.position, endPos, lerp);
                float outDistanceToTarget = Vector3.Distance(tongue.transform.position, targetPos);
                float outSegmentDistance = outDistanceToTarget / tongue.positionCount;
                Vector3 outTargetDirection = (targetPos - tongue.transform.position).normalized;
                float outDistanceFromEnd = Vector3.Distance(targetPos, endPos);
                Vector2[] tonguePoints = new Vector2[tongue.positionCount];
                for (int i = 0; i < tongue.positionCount; i++)
                {
                    
                    tongue.SetPosition(i, tongue.transform.position + outTargetDirection * (i * outSegmentDistance) + Vector3.up * Mathf.Sin(i * outSegmentDistance) * Mathf.Sin(1 - (outDistanceFromEndStart - outDistanceFromEnd) / outDistanceFromEndStart));
                    tonguePoints[i] = (Vector2)tongue.GetPosition(i) - (Vector2)tongue.transform.position;
                }
                
                
                edgeCollider.points = tonguePoints;
                RaycastHit2D tongueHit = Physics2D.Raycast(tongue.transform.position, outTargetDirection, outDistanceToTarget, playerMask);
                if (tongueHit)
                {
                    if(tongueHit.collider.gameObject.GetComponent<ItemScript>() && !tongueHit.collider.gameObject.name.Contains("Sword")){
                        tongueHit.collider.gameObject.GetComponent<ItemScript>().Break();
                    } else if(tongueHit.collider.gameObject.GetComponent<PlayerController>()){
                        tongueHit.collider.gameObject.GetComponent<PlayerController>().Capture();
                        tongueHit.collider.gameObject.GetComponent<PlayerController>().Kill();
                        capturedPlayer = true;

                    }
                    
                    
                    lerp = 0;
                    endPos = playerTransform.position;
                    
                    tongueState = TongueStates.goingIn;
                    myAnim.SetTrigger("Retract");
                    return;
                }
                if(outDistanceToTarget >= Vector3.Distance(tongue.transform.position,endPos)){
                    lerp = 0;
                    
                    tongueState = TongueStates.goingIn;
                    myAnim.SetTrigger("Retract");
                }
                break;
        }
        if (!capturedPlayer)
        {
            

            // tongue.SetPosition(0,tongue.transform.position);
            // tongue.SetPosition(1,target.position);
        }
        else
        {

        }

    }

    public void ActivateTongue()
    {
        
        endPos = playerTransform.position + Vector3.up;
        outDistanceFromEndStart = Vector3.Distance(endPos,tongue.transform.position);
        distanceFromEndStart = outDistanceFromEndStart;
        tongueState = TongueStates.goingOut;
        lerp = 0;
        targetPos = tongue.transform.position;
        attacking = true;
        for(int i = 0; i < tongue.positionCount;i++){
            tongue.SetPosition(i,tongue.transform.position);
        }
        tongue.gameObject.SetActive(true);
    }

    public void Retract(){
        tongueState = TongueStates.goingIn;
        myAnim.SetTrigger("Retract");
    }

    
}
