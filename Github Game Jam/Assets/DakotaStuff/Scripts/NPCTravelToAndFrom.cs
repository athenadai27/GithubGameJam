using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTravelToAndFrom : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public Transform endPosTransform;
    public float waitTime;
    public TextControllerV2 textController;
    public enum TravelStates {going, coming, waiting, nothing, attacking};
    public TravelStates travelState;
    public float moveLerp;
    public float secondsToGetThere;
    public bool waiting;
    public Animator frogAnim;
    public Canvas frogCanvas;
    public bool doneScouting;
    public EnemyAlertTutorial enemyAlertScript;
    public FrogGruntAttackTest frogAttackScript;
    public Vector3 nextPhaseTransform;
    public TeachingHowToGrabNPC teachingHowToGrabNPC;
    public Transform teleportPos;
    public GameObject nextPromptColliderObject;
    public HoujeilahScript houjeilah;
    public GameObject nextPrompt;
    public CircleCollider2D aggroCollider;
    public FrogGruntAttackTest tongueAttack;
    public LayerMask playerMask;
    // Start is called before the first frame update
    void Start()
    {
        endPos = endPosTransform.position;
        StartGoing(endPos);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerOverlap = Physics2D.OverlapCircle(aggroCollider.bounds.center,aggroCollider.radius,playerMask);
        if(playerOverlap && travelState != TravelStates.attacking){
            playerOverlap.GetComponent<PlayerController>().canMove = false;
            frogAnim.SetTrigger("Attack");
            travelState = TravelStates.attacking;
        }
        switch(travelState){
            
            case TravelStates.going:
                moveLerp += Time.deltaTime/secondsToGetThere;

                Vector3 nextPos = Vector3.Lerp(startPos,endPos,moveLerp);
                if(nextPos.x > transform.position.x){
                    transform.localScale = new Vector3(-1,1,1);
                } else if(nextPos.x < transform.position.x){
                    transform.localScale = Vector3.one;
                }
                frogCanvas.transform.localScale = transform.localScale;
                transform.position = nextPos;
                if(moveLerp >= 1){
                    if(doneScouting){
                        travelState = TravelStates.nothing;
                        teachingHowToGrabNPC.enabled = true;
                        transform.position = teleportPos.position;
                        transform.localScale = Vector3.one;
                        frogCanvas.transform.localScale = transform.localScale;
                       // enemyAlertScript.enabled = true;
                       // frogAttackScript.enabled = true;
                        nextPromptColliderObject.SetActive(true);
                        houjeilah.Appear();
                        nextPrompt.SetActive(true);
                        Debug.Log("houjeilahappear");
                        frogAnim.SetBool("Sleeping",true);
                        frogAnim.SetBool("Walking",false);
                        this.enabled = false;
                        return;
                    }
                    travelState = TravelStates.waiting;
                    textController.gameObject.SetActive(true);
                    frogAnim.SetBool("Walking",false);
                    
                }
                break;

            case TravelStates.waiting:
                if(textController.CheckIfArrived() && !waiting){
                    waitTime = Time.time + 1f;
                    waiting = true;
                } else if(waiting && Time.time > waitTime){
                    textController.FadeText();
                    textController.gameObject.SetActive(false);
                    StartGoing(startPos);
                    doneScouting = true;
                }
                break;
            case TravelStates.nothing:
                break;
            case TravelStates.attacking:
                break;
        }
    }

    public void StartGoing(Vector3 targetPos){
        startPos = transform.position;
        endPos = targetPos;
        moveLerp = 0f;
        travelState = TravelStates.going;
        frogAnim.SetBool("Walking",true);
    }
}
