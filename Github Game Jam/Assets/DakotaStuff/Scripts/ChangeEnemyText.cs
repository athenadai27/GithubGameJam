using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyText : MonoBehaviour
{
    public TextControllerV2 waitingOnTextController;
    public float waitTime;
    public bool waiting;
    public EnemyAlertTutorial enemyAlertTutorial;
    public FrogGruntAttackTest enemyTongue;
    
    public TextControllerV2 currentEnemyText;
    public TextControllerV2 nextEnemyText;
    public GameObject waitForNextRetractObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(waitingOnTextController.CheckIfArrived() && !waiting){
            waiting = true;
            waitTime = Time.time + 1f;
        } else if(waiting && Time.time > waitTime){
            currentEnemyText.FadeText();
            nextEnemyText.gameObject.SetActive(true);
            currentEnemyText.enabled = true;
            nextEnemyText.enabled = true;
            waitForNextRetractObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
