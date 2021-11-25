using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAlert : MonoBehaviour
{
    public enum AlertLevels { sleeping, idle, searching, pursuit, wakingUp, attacking };
    public AlertLevels alertLevel;
    public int currentThreatLevel;
    public List<GameObject> sentenceControllers;
    public GameObject activeSentenceController;
    public TextControllerV2 textController;

    public Transform playerTransform;
    public float moveSpeed;

    public Transform searchTransform;
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
    // Start is called before the first frame update
    void Start()
    {
        myAnim.SetBool("Sleeping", true);
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerDetectCollider = Physics2D.OverlapCircle(playerDetector.bounds.center, playerDetector.radius, playerMask);
        if (playerDetectCollider && alertLevel != AlertLevels.wakingUp)
        {
            Debug.Log("fight");
            SetThreatLevel(3);

        }
        switch (alertLevel)
        {
            case AlertLevels.idle:

                if (Vector3.Distance(transform.position, startingPos) > 1)
                {
                    if (startingPos.x < transform.position.x)
                    {
                        if (npcSprite.flipX)
                        {
                            npcSprite.flipX = false;
                        }
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (startingPos.x > transform.position.x)
                    {
                        if (!npcSprite.flipX)
                        {
                            npcSprite.flipX = true;
                        }
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    if (!invokingThreatLevel)
                    {
                        myAnim.SetBool("Walking", false);
                        npcSprite.flipX = false;
                        invokingThreatLevel = true;
                        StopCoroutine("ThreatCoroutine");
                        StartCoroutine(ThreatCoroutine(0, 5f));
                    }
                }

                break;
            case AlertLevels.wakingUp:
                break;
            case AlertLevels.searching:

                if (Vector3.Distance(transform.position, searchTransform.position) > 1)
                {
                    if (myAnim.GetBool("Sleeping"))
                    {
                        myAnim.SetBool("Sleeping", false);
                    }
                    if (!myAnim.GetBool("Walking"))
                    {
                        myAnim.SetBool("Walking", true);
                    }
                    if (searchTransform.position.x < transform.position.x)
                    {
                        if (npcSprite.flipX)
                        {
                            npcSprite.flipX = false;
                        }
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (searchTransform.position.x > transform.position.x)
                    {
                        if (!npcSprite.flipX)
                        {
                            npcSprite.flipX = true;
                        }
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    if (!invokingThreatLevel)
                    {
                        invokingThreatLevel = true;
                        StopCoroutine("ThreatCoroutine");
                        StartCoroutine(ThreatCoroutine(1, 5f));
                    }
                    if (myAnim.GetBool("Walking"))
                    {
                        myAnim.SetBool("Walking", false);
                    }
                }


                break;
            case AlertLevels.pursuit:
                Vector3 playerDirection = (playerTransform.position + Vector3.up) - transform.position;
                //  Debug.Log(playerDirection.magnitude);
                RaycastHit2D targetPlayerRay = Physics2D.Raycast(transform.position, playerDirection, playerDirection.magnitude + 50f, playerMask);
                if (!targetPlayerRay)
                {
                    //Debug.Log("STOP HAMMER TIME");
                    if (!invokingThreatLevel)
                    {
                        invokingThreatLevel = true;
                        StopCoroutine("ThreatCoroutine");
                        StartCoroutine(ThreatCoroutine(4, 2f));

                    }
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
                else
                {
                    if (playerTransform.position.x < transform.position.x)
                    {
                        if (npcSprite.flipX)
                        {
                            npcSprite.flipX = false;
                        }
                        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    }
                    else if (playerTransform.position.x > transform.position.x)
                    {
                        if (!npcSprite.flipX)
                        {
                            npcSprite.flipX = true;
                        }
                        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    }
                }


                break;
            case AlertLevels.attacking:
                break;
            case AlertLevels.sleeping:
                if (!myAnim.GetBool("Sleeping"))
                {
                    myAnim.SetBool("Sleeping", true);
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // if (collider.gameObject.CompareTag("AlertLight"))
        // {
        //     int threatLevel = collider.gameObject.GetComponent<AlertLight>().threatLevel;

        //     SetThreatLevel(threatLevel);

        // }
    }

    public void DeactivateText()
    {
        activeSentenceController.SetActive(false);
        if (textController != null)
        {
            for (int i = 0; i < textController.idleBlockList.Count; i++)
            {
                textController.idleBlockList[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetThreatLevel(int threatLevel)
    {
        Debug.Log(threatLevel);
        if (currentThreatLevel != threatLevel)
        {
            activeSentenceController.SetActive(false);
            if (textController != null)
            {
                for (int i = 0; i < textController.idleBlockList.Count; i++)
                {
                    textController.idleBlockList[i].gameObject.SetActive(false);
                }
            }


        }
        currentThreatLevel = threatLevel;
        if (alertLevel == AlertLevels.sleeping && currentThreatLevel != 0)
        {
            StartWakeUp();
            return;
        }
        textController = sentenceControllers[threatLevel].GetComponent<TextControllerV2>();
        if (threatLevel == 0)
        {
            myAnim.SetBool("Walking", false);
            sentenceControllers[0].SetActive(true);
            activeSentenceController = sentenceControllers[0];
            alertLevel = AlertLevels.sleeping;

        }
        else if (threatLevel == 1)
        {

            alertLevel = AlertLevels.idle;
            sentenceControllers[1].SetActive(true);
            activeSentenceController = sentenceControllers[1];

        }
        else if (threatLevel == 2)
        {
            alertSign.SetActive(true);

            alertLevel = AlertLevels.searching;
            sentenceControllers[2].SetActive(true);
            activeSentenceController = sentenceControllers[2];

        }
        else if (threatLevel == 3)
        {
            Debug.Log("pursuit");
            alertLevel = AlertLevels.pursuit;
            sentenceControllers[3].SetActive(true);
            activeSentenceController = sentenceControllers[3];
        }
        else if(threatLevel == 4){
            alertLevel = AlertLevels.idle;
            sentenceControllers[4].SetActive(true);
            activeSentenceController = sentenceControllers[4];
        }
    }

    public void StartWakeUp()
    {
        myAnim.SetBool("Sleeping", false);
        alertLevel = AlertLevels.wakingUp;


    }

    public void FinishWakeUp()
    {
        myAnim.SetBool("Walking", true);
        Debug.Log("finishwakeup");
        SetThreatLevel(currentThreatLevel);
    }

    IEnumerator ThreatCoroutine(int newThreatLevel, float delayTime)
    {
        Debug.Log("threatroutine");
        yield return new WaitForSeconds(delayTime);
        invokingThreatLevel = false;
        SetThreatLevel(newThreatLevel);
    }

    public void TongueDone()
    {
        currentThreatLevel = 3;
        alertLevel = AlertLevels.pursuit;
    }
}
