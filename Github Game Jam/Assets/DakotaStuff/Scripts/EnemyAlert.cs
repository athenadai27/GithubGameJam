﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAlert : EnemyHealth
{
    public enum AlertLevels { sleeping, backToStart, searching, pursuit, wakingUp, attacking, lured, performingAction, suspicious, waitForNextState };

    public AlertLevels alertLevel;

    public GameObject sleepText;
    public GameObject searchText;
    public GameObject boutheinaSpottedText;
    public GameObject luredText;
    public GameObject chillText;
    public GameObject currentTextController;
    public GameObject suspiciousText;
    public TextControllerV2 textController;

    public Transform playerTransform;
    public float moveSpeed;

    public Transform lureTransform;
    public SpriteRenderer npcSprite;

    public GameObject alertSign;
    public float wakeUpTime;

    [SerializeField]
    private Animator myAnim;

    [SerializeField]
    private CircleCollider2D playerDetector;
    [SerializeField]
    private LayerMask playerMask;

    [SerializeField]
    private AlertLevels nextAlertLevel;

    [SerializeField]
    private Vector3 startingPos;

    [SerializeField]
    bool invokingThreatLevel;

    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float attackCooldownAdditional;

    public float delayTime;
    public bool waiting;
    public GameObject nextText;
    public FrogGruntAttackTest tongueAttack;
    public Transform canvas;
    public LayerMask eventMask;
    public BoxCollider2D bodyCollider;
    public string objectToDestroyName;
    public bool sleeper;
    public Transform tongueTransform;
    public Vector3 spawnPos;
    public bool badEyesight;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (sleeper)
        {
            myAnim.SetBool("Sleeping", true);
        }

        startingPos = transform.position;
        spawnPos = transform.position;
        alertLevel = AlertLevels.sleeping;
        ActivateTextController(sleepText);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerDetectCollider = Physics2D.OverlapCircle(playerDetector.bounds.center, playerDetector.radius, playerMask);
        if (playerDetectCollider && alertLevel != AlertLevels.wakingUp && alertLevel != AlertLevels.pursuit && alertLevel != AlertLevels.attacking)
        {
            Debug.Log("pursuit");
            if(!playerDetectCollider.gameObject.GetComponentInChildren<StemController>().hasFrogMask || !badEyesight){
                nextAlertLevel = AlertLevels.pursuit;
                if (alertLevel != AlertLevels.sleeping || !sleeper)
                {
                    alertLevel = AlertLevels.waitForNextState;
                }

                ActivateTextController(boutheinaSpottedText);
            } else if(playerDetectCollider.gameObject.GetComponentInChildren<StemController>().hasFrogMask && badEyesight){
                alertLevel = AlertLevels.sleeping;
                ActivateTextController(chillText);
                bodyCollider.enabled = false;
            }
            
        }
        switch (alertLevel)
        {
            case AlertLevels.backToStart:

                if (Vector3.Distance(transform.position, startingPos) > 1)
                {
                    if (startingPos.x < transform.position.x)
                    {
                        transform.localScale = Vector3.one;
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (startingPos.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                    tongueTransform.localScale = transform.localScale;
                    canvas.transform.localScale = transform.localScale;
                }
                else
                {
                    myAnim.SetBool("Walking", false);
                    transform.localScale = Vector3.one;
                    canvas.transform.localScale = transform.localScale;
                    tongueTransform.localScale = transform.localScale;
                    if (sleeper)
                    {
                        myAnim.SetBool("Sleeping", true);
                        alertLevel = AlertLevels.sleeping;
                    }

                    ActivateTextController(sleepText);
                    waiting = false;

                }
                break;
            case AlertLevels.waitForNextState:
                if (Time.time > delayTime && waiting)
                {
                    alertLevel = nextAlertLevel;
                    switch (nextAlertLevel)
                    {
                        case AlertLevels.suspicious:
                            myAnim.SetBool("Walking", true);
                            alertLevel = AlertLevels.backToStart;
                            break;
                        case AlertLevels.pursuit:
                            myAnim.SetBool("Walking", true);
                            break;
                        case AlertLevels.lured:
                            myAnim.SetBool("Walking", true);
                            break;
                        case AlertLevels.backToStart:
                            myAnim.SetBool("Walking", true);
                            break;
                    }
                }
                else if (textController.CheckIfArrived() && !waiting)
                {
                    waiting = true;
                    delayTime = 1f;
                }
                break;
            case AlertLevels.wakingUp:
                break;
            case AlertLevels.lured:

                if (Vector3.Distance(transform.position, lureTransform.position) > 1)
                {

                    if (lureTransform.position.x < transform.position.x)
                    {
                        transform.localScale = Vector3.one;
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (lureTransform.position.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                    canvas.transform.localScale = transform.localScale;
                    tongueTransform.localScale = transform.localScale;
                }
                else
                {
                    if (myAnim.GetBool("Walking"))
                    {
                        myAnim.SetBool("Walking", false);
                    }
                    delayTime = Time.time + 5f;
                    alertLevel = AlertLevels.performingAction;
                }
                break;
            case AlertLevels.performingAction:
                if (Time.time > delayTime)
                {
                    alertLevel = AlertLevels.waitForNextState;
                    nextAlertLevel = AlertLevels.backToStart;
                    ActivateTextController(chillText);
                    Collider2D overlapBody = Physics2D.OverlapBox(bodyCollider.bounds.center, bodyCollider.bounds.size, 0, eventMask);
                    if (overlapBody)
                    {
                        if (overlapBody.gameObject.name.Contains(objectToDestroyName))
                        {
                            Destroy(overlapBody.gameObject);
                        }

                    }
                }
                break;
            case AlertLevels.pursuit:
                Vector3 playerDirection = (playerTransform.position + Vector3.up) - transform.position;
                //  Debug.Log(playerDirection.magnitude);
                RaycastHit2D targetPlayerRay = Physics2D.Raycast(transform.position, playerDirection, playerDirection.magnitude + 50f, playerMask);
                if (!targetPlayerRay)
                {
                    Debug.Log("suspicious");
                    nextAlertLevel = AlertLevels.suspicious;
                    if (alertLevel != AlertLevels.sleeping  || !sleeper)
                    {

                        alertLevel = AlertLevels.waitForNextState;
                    }

                    ActivateTextController(suspiciousText);

                    waiting = false;
                    return;
                }
                if (Vector3.Distance(playerTransform.position, transform.position) < 10f)
                {
                    if (Time.time > attackCooldown)
                    {
                        attackCooldown = Time.time + attackCooldownAdditional;
                        myAnim.SetTrigger("Attack");
                        alertLevel = AlertLevels.attacking;
                    }
                }
                else if(Vector3.Distance(playerTransform.position, transform.position) > 20f){
                     nextAlertLevel = AlertLevels.suspicious;
                    if (alertLevel != AlertLevels.sleeping  || !sleeper)
                    {

                        alertLevel = AlertLevels.waitForNextState;
                    }

                    ActivateTextController(suspiciousText);

                    waiting = false;
                    return;
                }
                else
                {
                    if (playerTransform.position.x < transform.position.x)
                    {
                        transform.localScale = Vector3.one;
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (playerTransform.position.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                    canvas.transform.localScale = transform.localScale;
                    tongueTransform.localScale = transform.localScale;
                }


                break;
            case AlertLevels.attacking:
                if (!tongueAttack.attacking)
                {
                    if (playerTransform.position.x < transform.position.x)
                    {
                        transform.localScale = Vector3.one;
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (playerTransform.position.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                    canvas.transform.localScale = transform.localScale;
                    tongueTransform.localScale = transform.localScale;
                }
                break;
            case AlertLevels.sleeping:

                break;
        }
    }


    public void DeactivateText()
    {
        currentTextController.SetActive(false);
        if (textController != null)
        {
            for (int i = 0; i < textController.idleBlockList.Count; i++)
            {
                textController.idleBlockList[i].gameObject.SetActive(false);
            }
        }
    }

    public void ActivateTextController(GameObject newTextController)
    {
        if (currentTextController != null)
        {
            if (currentTextController != newTextController)
            {
                currentTextController.SetActive(false);
                if (textController != null)
                {
                    textController.FadeText();
                }


            }
        }

        currentTextController = newTextController;
        textController = currentTextController.GetComponent<TextControllerV2>();
        if ((alertLevel == AlertLevels.sleeping && currentTextController != sleepText) && sleeper)
        {

            StartWakeUp();
            nextText = currentTextController;
            return;
        }
        currentTextController.SetActive(true);
    }

    public void StartWakeUp()
    {
        myAnim.SetBool("Sleeping", false);
        alertLevel = AlertLevels.wakingUp;


    }

    public void FinishWakeUp()
    {
        ActivateTextController(currentTextController);
        alertLevel = AlertLevels.waitForNextState;
    }



    public void TongueDone()
    {
        currentTextController = boutheinaSpottedText;
        alertLevel = AlertLevels.pursuit;
        Debug.Log("pursuit");
    }

    public void Lure()
    {
        nextAlertLevel = AlertLevels.lured;
        if (alertLevel != AlertLevels.sleeping)
        {


            alertLevel = AlertLevels.waitForNextState;
        }
        ActivateTextController(luredText);

    }

    public override void Reset()
    {
        base.Reset();
        tongueAttack.Reset();
        alertLevel = AlertLevels.sleeping;
        nextAlertLevel = AlertLevels.sleeping;
        textController.FadeText();
        ActivateTextController(sleepText);

        myAnim.Rebind();
        transform.position = spawnPos;
        transform.localScale = Vector3.one;
        canvas.transform.localScale = transform.localScale;
        tongueTransform.localScale = transform.localScale;

    }

}
