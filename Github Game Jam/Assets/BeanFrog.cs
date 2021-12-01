using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanFrog : EnemyHealth
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
    public Transform canvas;
    public LayerMask eventMask;
    public BoxCollider2D bodyCollider;
    public string objectToDestroyName;
    public bool sleeper;
    public Vector3 spawnPos;
    public GameObject projectileHolder;
    public Transform projectileTransform;
    public float groundY;
    public Transform parentTransform;
    public Vector3 resetScale;
    public AudioSource beanAudioSource;
    public AudioClip launchClip;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        if (sleeper)
        {
            myAnim.SetBool("Sleeping", true);
        }
        resetScale = parentTransform.localScale;
        startingPos = parentTransform.position;
        spawnPos = parentTransform.position;
        alertLevel = AlertLevels.sleeping;
        ActivateTextController(sleepText);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerDetectCollider = Physics2D.OverlapCircle(playerDetector.bounds.center, playerDetector.radius, playerMask);
        if (playerDetectCollider && alertLevel != AlertLevels.wakingUp && alertLevel != AlertLevels.pursuit && alertLevel != AlertLevels.attacking)
        {
            nextAlertLevel = AlertLevels.pursuit;
            if (alertLevel != AlertLevels.sleeping || !sleeper)
            {
                alertLevel = AlertLevels.waitForNextState;
            }
            
            ActivateTextController(boutheinaSpottedText);
        }
        switch (alertLevel)
        {
            case AlertLevels.backToStart:

                if (Vector3.Distance(parentTransform.position, startingPos) > 1)
                {
                    if (startingPos.x < parentTransform.position.x)
                    {
                        parentTransform.localScale = Vector3.one;
                        parentTransform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (startingPos.x > parentTransform.position.x)
                    {
                        parentTransform.localScale = new Vector3(-1, 1, 1);
                        parentTransform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                    canvas.transform.localScale = parentTransform.localScale;
                }
                else
                {
                    myAnim.SetBool("Walking", false);
                    parentTransform.localScale = Vector3.one;
                    canvas.transform.localScale = parentTransform.localScale;
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

                if (Vector3.Distance(parentTransform.position, lureTransform.position) > 1)
                {

                    if (lureTransform.position.x <parentTransform.position.x)
                    {
                       parentTransform.localScale = Vector3.one;
                       parentTransform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (lureTransform.position.x > parentTransform.position.x)
                    {
                        parentTransform.localScale = new Vector3(-1, 1, 1);
                        parentTransform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                    canvas.transform.localScale = parentTransform.localScale;
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
                Vector3 playerDirection = (playerTransform.position + Vector3.up) - parentTransform.position;
                playerDirection.Normalize();
                //  Debug.Log(playerDirection.magnitude);
                RaycastHit2D targetPlayerRay = Physics2D.Raycast(parentTransform.position, playerDirection, playerDirection.magnitude + 25f, playerMask);
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
                }else{
                    
                }
                if (Vector3.Distance(playerTransform.position, parentTransform.position) < 25f)
                {
                    if (Time.time > attackCooldown)
                    {
                        attackCooldown = Time.time + attackCooldownAdditional;
                        myAnim.SetTrigger("Attack");
                        alertLevel = AlertLevels.attacking;
                    }
                }
                else
                {
                    if (playerTransform.position.x < parentTransform.position.x)
                    {
                        parentTransform.localScale = Vector3.one;
                        parentTransform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (playerTransform.position.x > parentTransform.position.x)
                    {
                        parentTransform.localScale = new Vector3(-1, 1, 1);
                        parentTransform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                    canvas.transform.localScale = parentTransform.localScale;
                }


                break;
            case AlertLevels.attacking:

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



    public void Lure()
    {
        nextAlertLevel = AlertLevels.lured;
        if (alertLevel != AlertLevels.sleeping)
        {


            alertLevel = AlertLevels.waitForNextState;
        }
        ActivateTextController(luredText);

    }

    public void LaunchDice()
    {
        
        GameObject newSlime = projectileHolder.transform.GetChild(0).gameObject;
        newSlime.transform.position = projectileTransform.position;
        newSlime.transform.up = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        ProjectileScript projectile = newSlime.GetComponent<ProjectileScript>();

        projectile.startPos = projectileTransform.position;
        
        projectile.endPos = new Vector3(playerTransform.position.x, groundY, 0f);
    
        float projectileLerpHeight = Random.Range(10f, 15f);
        projectile.midPoint = projectile.startPos + (projectile.endPos - projectile.startPos) / 2 + Vector3.up * projectileLerpHeight;
        projectile.lerp = 0f;

        newSlime.SetActive(true);
        newSlime.transform.SetAsLastSibling();

        alertLevel = AlertLevels.pursuit;
        
    }

    public override void Reset()
    {
        base.Reset();
        alertLevel = AlertLevels.sleeping;
        ActivateTextController(sleepText);
        myAnim.Rebind();
        parentTransform.position = spawnPos;
        parentTransform.localScale = resetScale;
        canvas.transform.localScale = parentTransform.localScale;

    }

}
