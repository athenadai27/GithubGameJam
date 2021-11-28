using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMiniboss : MonoBehaviour
{
    public enum BossStates { leapUp, leapDown, croaking, sliming, tongueing, prepareToTongue, ouchie, stunned, prepareToJump, prepareToFall, prepareForSliming, prepareForCroaking, phase1, phase2, changingPhases, doneFighting, goodbye }
    public BossStates bossState;
    public BossStates currentPhase;
    public Vector3 lerpStartPos;
    public Vector3 lerpEndPos;
    public float leapLerp;
    public FallCircleTest fallCircle;
    public Transform playerTransform;
    public BoxCollider2D boxCollider;
    public LayerMask fallMask;
    public Transform midpointTransform;
    public float ouchieTopY;
    public Vector3 ouchieMidpoint;

    public Animator myAnim;
    public bool croak1;
    public GameObject sonicWavesHolder;
    public GameObject oozeHolder;
    public List<ToxicSlimeProjectile> toxicSlimes = new List<ToxicSlimeProjectile>();
    public List<SoundWave> soundWaves = new List<SoundWave>();
    public float shakeSpawnTime;
    public int slimeTracker;
    public int croakTracker;
    public bool doneCroaking;
    public GameObject smokeCloud;

    public Transform diceSpawnTransform;

    public List<GameObject> textControllers;
    public List<GameObject> tongueTexts;
    public List<GameObject> jumpTexts;
    public List<GameObject> diceTexts;
    public List<GameObject> sonicTexts;
    public GameObject phaseChangeText;
    public GameObject fightOverText;
    public GameObject activeTextController;
    public TextControllerV2 textControllerScript;

    public float delayTime;

    public FrogGruntAttackTest tongueScript;
    public bool tongueAttack;
    public GameObject canvas;
    public Checkpoint checkpoint;
    public bool resetting;
    public EnemyHealth enemyHealth;
    public List<int> chances;
    public BoxCollider2D areaBounds;
    public int previousSimilarTextChosen;
    public bool doneFighting;
    public float groundY;
    public FollowPlayer camFollow;
    public bool cameraShaking;
    public bool waiting;
    public Transform croakTransform;

    public int numWavesPerCroak;
    public int numCroaks;
    public int numShakes;
    // Start is called before the first frame update
    void Start()
    {
        groundY = transform.position.y;
        // for(int i = 0; i < oozeHolder.transform.childCount;i++){
        //     toxicSlimes.Add(oozeHolder.transform.GetChild(i).GetComponent<ToxicSlimeProjectile>());
        // }
        // for(int i = 0; i < sonicWavesHolder.transform.childCount;i++){
        //     soundWaves.Add(sonicWavesHolder.transform.GetChild(i).GetComponent<SoundWave>());
        // }
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.localScale = transform.localScale;
        
        switch (bossState)
        {
            case BossStates.leapUp:
                leapLerp += Time.deltaTime * 1.5f;
                transform.position = Vector3.Lerp(lerpStartPos, lerpEndPos, leapLerp);
                if (transform.position == lerpEndPos)
                {
                    fallCircle.gameObject.SetActive(false);
                    bossState = BossStates.prepareToFall;
                    delayTime = Time.time + 3f;
                    if (doneFighting)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
                break;
            case BossStates.leapDown:
                leapLerp += Time.deltaTime;
                Vector3 nextPos = Vector3.Lerp(lerpStartPos, lerpEndPos, leapLerp);
                RaycastHit2D fallCast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, Vector2.Distance(nextPos, transform.position), fallMask);
                if (fallCast)
                {
                    if (fallCast.collider.CompareTag("Player") && !resetting)
                    {
                        resetting = true;
                        checkpoint.Reset();

                    }
                    else if (fallCast.collider.CompareTag("Ground"))
                    {
                        myAnim.SetBool("Jumping", false);
                        Debug.Log("shake");
                        if(!camFollow.cameraShaking && !cameraShaking){
                            StartCoroutine(camFollow.CameraShake(.5f,.1f));
                            cameraShaking = true;
                        }
                        
                    }
                    else if (fallCast.collider.CompareTag("HeldItem") && !fallCast.collider.gameObject.name.Contains("Sword"))
                    {
                        fallCast.collider.gameObject.GetComponent<ItemScript>().Break();
                        ItemHit();
                    }
                    


                }
                transform.position = nextPos;
                if (transform.position == lerpEndPos)
                {
                    fallCircle.gameObject.SetActive(false);
                }
                break;
            case BossStates.ouchie:
                leapLerp += Time.deltaTime;
                Vector3 m1 = Vector3.Lerp(lerpStartPos, ouchieMidpoint, leapLerp);
                Vector3 m2 = Vector3.Lerp(ouchieMidpoint, lerpEndPos, leapLerp);
                transform.position = Vector3.Lerp(m1, m2, leapLerp);
                if (transform.position == lerpEndPos)
                {
                    fallCircle.gameObject.SetActive(false);
                    myAnim.SetBool("Jumping", false);
                    if(!camFollow.cameraShaking && !cameraShaking){
                        Debug.Log("shake");
                        StartCoroutine(camFollow.CameraShake(.5f,.1f));
                        cameraShaking = true;
                    }
                    
                }
                break;
            case BossStates.croaking:
                if (doneCroaking)
                {
                    bool allOffScreen = true;
                    for (int i = 0; i < sonicWavesHolder.transform.childCount; i++)
                    {
                        if (sonicWavesHolder.transform.GetChild(i).gameObject.activeSelf)
                        {
                            if (sonicWavesHolder.transform.GetChild(i).transform.position.x < areaBounds.bounds.max.x &&
                            sonicWavesHolder.transform.GetChild(i).transform.position.x > areaBounds.bounds.min.x &&
                            sonicWavesHolder.transform.GetChild(i).transform.position.y < areaBounds.bounds.max.y &&
                            sonicWavesHolder.transform.GetChild(i).transform.position.y > areaBounds.bounds.min.y)
                            {
                                allOffScreen = false;
                            }
                        }
                    }
                    if (allOffScreen)
                    {
                        doneCroaking = false;
                        bossState = BossStates.phase2;
                    }
                }
                break;
            case BossStates.prepareForCroaking:
                if (Time.time > delayTime && waiting)
                {
                    textControllerScript.FadeText();
                    bossState = BossStates.croaking;
                    croakTracker = 0;
                    myAnim.SetBool("Croaking", true);
                    waiting = false;
                } else{
                    if(textControllerScript.CheckIfArrived() && !waiting){
                        waiting = true;
                        delayTime = Time.time + 1f;
                    }
                    
                }
                break;
            
            case BossStates.sliming:

                break;
            case BossStates.prepareForSliming:
                if (Time.time > delayTime && waiting)
                {
                    textControllerScript.FadeText();
                    myAnim.SetBool("Shaking", true);
                    bossState = BossStates.sliming;
                    slimeTracker = 0;
                    waiting = false;
                } else{
                    if( textControllerScript.CheckIfArrived() && !waiting){
                        waiting = true;
                        delayTime = Time.time + 1f;
                        
                    }
                    
                }
                break;
            case BossStates.tongueing:
                if (tongueScript.tongueState == FrogGruntAttackTest.TongueStates.goingIn && !tongueAttack)
                {
                    tongueAttack = true;
                }
                if (tongueScript.tongueState == FrogGruntAttackTest.TongueStates.dormant && tongueAttack)
                {
                    myAnim.SetBool("Tonguing", false);
                    bossState = currentPhase;
                    tongueAttack = false;
                }
                else if (tongueScript.tongueState == FrogGruntAttackTest.TongueStates.goingIn && myAnim.GetCurrentAnimatorStateInfo(0).IsName("TongueOut"))
                {
                    myAnim.SetTrigger("TongueDone");
                }
                break;
            case BossStates.prepareToTongue:
                if (playerTransform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);


                }
                else if (playerTransform.position.x < transform.position.x)
                {
                    transform.localScale = Vector3.one;

                }
                tongueScript.tongue.transform.localScale = transform.localScale;
                canvas.transform.localScale = transform.localScale;
                if (Time.time > delayTime && waiting)
                {
                    textControllerScript.FadeText();
                    bossState = BossStates.tongueing;
                    myAnim.SetBool("Tonguing", true);
                    waiting = false;
                } else{
                    if(textControllerScript.CheckIfArrived() && !waiting){
                        waiting = true;
                        delayTime = Time.time + 1f;
                    }
                    
                }

                break;
            case BossStates.stunned:
                break;
            case BossStates.prepareToJump:
                if (Time.time > delayTime && waiting)
                {
                    textControllerScript.FadeText();
                    myAnim.SetBool("Jumping", true);
                    waiting = false;
                 } else{
                    if(textControllerScript.CheckIfArrived() && !waiting){
                        waiting = true;
                        delayTime = Time.time + 1f;
                    }
                    
                }
                break;
            case BossStates.prepareToFall:
                if (Time.time > delayTime && waiting)
                {
                    Fall();
                    waiting = false;
                } else{
                    if(textControllerScript.CheckIfArrived() && !waiting){
                        waiting = true;
                        delayTime = Time.time + 1f;
                    }
                    
                }
                break;
            case BossStates.phase1:
                if (enemyHealth.currentHealth <= 10)
                {
                    bossState = BossStates.changingPhases;
                    currentPhase = BossStates.changingPhases;
                    ActivateTextController(phaseChangeText);
                    delayTime = Time.time + 3f;
                    return;
                }
                float useTongue = Random.Range(0f, 1f);
                if (chances[0] >= 2)
                {
                    useTongue = .51f;
                }
                else if (chances[1] >= 2)
                {
                    useTongue = .5f;
                }
                int chosenIndex = 0;
                if (useTongue <= .5f)
                {

                    delayTime = Time.time + 3f;
                    bossState = BossStates.prepareToTongue;

                    chances[0]++;
                    if (previousSimilarTextChosen == 0)
                    {
                        ActivateTextController(tongueTexts[1]);
                        previousSimilarTextChosen = 1;
                    }
                    else
                    {
                        ActivateTextController(tongueTexts[0]);
                        previousSimilarTextChosen = 0;
                    }


                }
                else
                {
                    chosenIndex = 1;
                    chances[1]++;
                    bossState = BossStates.prepareToJump;
                    delayTime = Time.time + 3f;
                    float randomChance = Random.Range(0f, 1f);

                    if (previousSimilarTextChosen == 0)
                    {
                        ActivateTextController(jumpTexts[1]);
                        previousSimilarTextChosen = 1;
                    }
                    else
                    {
                        ActivateTextController(jumpTexts[0]);
                        previousSimilarTextChosen = 0;
                    }
                }
                for (int i = 0; i < chances.Count; i++)
                {
                    if (i != chosenIndex)
                    {
                        chances[i] = 0;
                    }
                }
                break;
            case BossStates.changingPhases:
                if (Time.time > delayTime && waiting)
                {
                    textControllerScript.FadeText();
                    bossState = BossStates.phase2;
                    waiting = false;
                } else{
                    if(textControllerScript.CheckIfArrived() && !waiting){
                        waiting = true;
                        delayTime = Time.time + 1f;
                    }
                    
                }
                break;
            case BossStates.doneFighting:
                if (Time.time > delayTime && waiting)
                {
                    doneFighting = true;
                    textControllerScript.FadeText();
                    myAnim.SetBool("Jumping", true);
                    waiting = false;
                } else{
                    if(textControllerScript.CheckIfArrived() && !waiting){
                        waiting = true;
                        delayTime = Time.time + 1f;
                    }
                    
                }
                break;
            case BossStates.goodbye:
                break;
            case BossStates.phase2:
                if (enemyHealth.currentHealth <= 0)
                {
                    bossState = BossStates.doneFighting;
                    currentPhase = BossStates.doneFighting;
                    ActivateTextController(fightOverText);
                    delayTime = Time.time + 6f;
                    return;
                }
                float randomAbilityChance = Random.Range(0f, 1f);
                List<int> randomChances = new List<int>();
                if (chances[0] < 1)
                {
                    randomChances.Add(0);
                }
                if (chances[1] < 1)
                {
                    randomChances.Add(1);
                }
                if (chances[2] < 1)
                {
                    randomChances.Add(2);
                }
                if (chances[3] < 1)
                {
                    randomChances.Add(3);
                }
                int randomIndex = Random.Range(0, randomChances.Count);

                int chosenPhase2Index = 0;

                // //change this after testing
                //  randomChances[randomIndex] = 3;
                if (randomChances[randomIndex] == 0)
                {
                    chances[0]++;
                    delayTime = Time.time + 3f;
                    bossState = BossStates.prepareToTongue;
                    float randomChance = Random.Range(0f, 1f);
                    if (previousSimilarTextChosen == 0)
                    {
                        ActivateTextController(tongueTexts[1]);
                        previousSimilarTextChosen = 1;
                    }
                    else
                    {
                        ActivateTextController(tongueTexts[0]);
                        previousSimilarTextChosen = 0;
                    }
                }
                else if (randomChances[randomIndex] == 1)
                {
                    chances[1]++;
                    chosenPhase2Index = 1;
                    bossState = BossStates.prepareToJump;
                    delayTime = Time.time + 3f;
                    float randomChance = Random.Range(0f, 1f);
                    if (previousSimilarTextChosen == 0)
                    {
                        ActivateTextController(jumpTexts[1]);
                        previousSimilarTextChosen = 1;
                    }
                    else
                    {
                        ActivateTextController(jumpTexts[0]);
                        previousSimilarTextChosen = 0;
                    }
                }
                else if (randomChances[randomIndex] == 2)
                {
                    chances[2]++;
                    chosenPhase2Index = 2;
                    bossState = BossStates.prepareForSliming;
                    delayTime = Time.time + 3f;
                    float randomChance = Random.Range(0f, 1f);
                    if (previousSimilarTextChosen == 0)
                    {
                        ActivateTextController(diceTexts[1]);
                        previousSimilarTextChosen = 1;
                    }
                    else
                    {
                        ActivateTextController(diceTexts[0]);
                        previousSimilarTextChosen = 0;
                    }
                }
                else if (randomChances[randomIndex] == 3)
                {
                    chances[3]++;
                    chosenPhase2Index = 3;
                    bossState = BossStates.prepareForCroaking;
                    delayTime = Time.time + 3f;
                    float randomChance = Random.Range(0f, 1f);
                    if (previousSimilarTextChosen == 0)
                    {
                        ActivateTextController(sonicTexts[1]);
                        previousSimilarTextChosen = 1;
                    }
                    else
                    {
                        ActivateTextController(sonicTexts[0]);
                        previousSimilarTextChosen = 0;
                    }
                }
                bool everyOptionChosen = true;
                for (int i = 0; i < chances.Count; i++)
                {
                    if (chances[i] == 0)
                    {
                        everyOptionChosen = false;
                    }
                }
                if (everyOptionChosen)
                {
                    for (int i = 0; i < chances.Count; i++)
                    {
                        chances[i] = 0;
                    }
                }
                break;
        }
    }

    public void Leap()
    {
        fallCircle.transform.position = new Vector3(transform.position.x, -3.21f, 0f);

        fallCircle.gameObject.SetActive(true);
        fallCircle.grow = false;

        leapLerp = 0;
        lerpStartPos = transform.position;
        lerpEndPos = transform.position + Vector3.up * 20f;
        bossState = BossStates.leapUp;
    }

    public void Fall()
    {

        transform.position = new Vector3(playerTransform.position.x, transform.position.y, 0);
        fallCircle.transform.position = new Vector3(transform.position.x, -3.21f, 0f);

        fallCircle.gameObject.SetActive(true);
        fallCircle.grow = true;

        leapLerp = 0;
        lerpStartPos = transform.position;
        lerpEndPos = transform.position - Vector3.up * 20f;
        bossState = BossStates.leapDown;
    }

    public void ItemHit()
    {
        lerpStartPos = transform.position;
        if (lerpStartPos.x > midpointTransform.position.x)
        {
            lerpEndPos = new Vector3(lerpStartPos.x - Random.Range(5f, 10f), groundY, 0f);
        }
        else
        {
            lerpEndPos = new Vector3(lerpStartPos.x + Random.Range(5f, 10f), groundY, 0f);
        }
        ouchieTopY = 5f;
        ouchieMidpoint = lerpStartPos + (lerpEndPos - lerpStartPos) / 2 + Vector3.up * ouchieTopY;
        leapLerp = 0f;
        bossState = BossStates.ouchie;
    }

    public void Croak()
    {
        croakTracker++;
        float angleChange = Random.Range(-45f, 45f);
        Vector3 vectorDif = playerTransform.position - (croakTransform.position);
        float radialDifference = 360/numWavesPerCroak;
        for (int i = 0; i < numWavesPerCroak; i++)
        {
            Vector3 newVector = Quaternion.Euler(0, 0, radialDifference * i + angleChange) * vectorDif;

            GameObject newWave = sonicWavesHolder.transform.GetChild(0).gameObject;
            newWave.transform.right = newVector;
            newWave.transform.position = croakTransform.position;
            // newWave.transform.localEulerAngles = new Vector3(0, 0, i * 90f + angleChange);
            newWave.SetActive(true);
            newWave.transform.SetAsLastSibling();
        }

        if (croakTracker >= numCroaks)
        {
            myAnim.SetBool("Croaking", false);
            //bossState = BossStates.phase2;
            doneCroaking = true;

        }
    }

    public void ToxicShake()
    {
        slimeTracker++;
        GameObject newSlime = oozeHolder.transform.GetChild(0).gameObject;
        newSlime.transform.position = diceSpawnTransform.position;
        newSlime.transform.up = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        ToxicSlimeProjectile toxicSlimeProjectile = newSlime.GetComponent<ToxicSlimeProjectile>();

        toxicSlimeProjectile.startPos = diceSpawnTransform.position;
        float aimAtBoutheina = Random.Range(0f, 1f);
        //List<float> currentActiveToxicPuddlePositions = new List<float>();
        
        toxicSlimeProjectile.endPos = new Vector3(toxicSlimeProjectile.startPos.x + Random.Range(-15f, 15f), -2.75f, 0f);
        if (aimAtBoutheina <= .25f)
        {
            toxicSlimeProjectile.endPos.x = playerTransform.position.x;
        } 
        bool tooCloseToOtherPuddles = false;
       
        for(int i = 0; i < oozeHolder.transform.childCount;i++){
            if(oozeHolder.transform.GetChild(i).gameObject.activeSelf){
                if(Vector3.Distance(toxicSlimeProjectile.endPos,oozeHolder.transform.GetChild(i).GetComponent<ToxicSlimeProjectile>().endPos) < 3){
                    tooCloseToOtherPuddles = true;
                }
            }
        }
        while(tooCloseToOtherPuddles){
            tooCloseToOtherPuddles = false;
            toxicSlimeProjectile.endPos = new Vector3(toxicSlimeProjectile.startPos.x + Random.Range(-15f, 15f), -2.75f, 0f);
             for(int i = 0; i < oozeHolder.transform.childCount;i++){

            if(oozeHolder.transform.GetChild(i).gameObject.activeSelf){
                if(Vector3.Distance(toxicSlimeProjectile.endPos,oozeHolder.transform.GetChild(i).GetComponent<ToxicSlimeProjectile>().endPos) < 3){
                    tooCloseToOtherPuddles = true;
                }
            }
        }
        }
        float oozeLerpHeight = Random.Range(10f, 15f);
        toxicSlimeProjectile.midPoint = toxicSlimeProjectile.startPos + (toxicSlimeProjectile.endPos - toxicSlimeProjectile.startPos) / 2 + Vector3.up * oozeLerpHeight;
        toxicSlimeProjectile.lerp = 0f;

        newSlime.SetActive(true);
        newSlime.transform.SetAsLastSibling();
        if (slimeTracker >= numShakes)
        {
            bossState = BossStates.phase2;
            myAnim.SetBool("Shaking", false);
        }
    }

    public void ActivateSmokeCloud()
    {
        Vector3 newSmokeCloudPosition = smokeCloud.transform.position;
        newSmokeCloudPosition.x = transform.position.x;
        smokeCloud.transform.position = newSmokeCloudPosition;

        smokeCloud.SetActive(true);
    }

    public void StartShaking()
    {
        bossState = BossStates.sliming;
    }


    public void ActivateTextController(GameObject sentTextController)
    {
        if (activeTextController != null)
        {
            if (activeTextController != sentTextController)
            {
                activeTextController.SetActive(false);
            }
        }


        activeTextController = sentTextController;


        textControllerScript = activeTextController.GetComponent<TextControllerV2>();
        if (transform.position.x > midpointTransform.position.x)
        {
            textControllerScript.relativeWordCloudCenter.x = -3;
        }
        else if (transform.position.x < midpointTransform.position.x)
        {
            textControllerScript.relativeWordCloudCenter.x = 3;
        }
        activeTextController.SetActive(true);
    }

    public void EndJump()
    {
        bossState = currentPhase;
        cameraShaking = false;
    }


    public void StartTongue()
    {

    }

    public void Reset()
    {
        if (activeTextController != null)
        {
            activeTextController.SetActive(false);
            activeTextController = null;
        }
        if (textControllerScript != null)
        {
            textControllerScript.FadeText();
            textControllerScript = null;
        }
        for (int i = 0; i < oozeHolder.transform.childCount; i++)
        {
            oozeHolder.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < sonicWavesHolder.transform.childCount; i++)
        {
            sonicWavesHolder.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        cameraShaking = false;
        currentPhase = BossStates.phase1;
        bossState = BossStates.phase1;
        myAnim.Rebind();
        tongueScript.tongue.gameObject.SetActive(false);
        resetting = false;
    }


}
