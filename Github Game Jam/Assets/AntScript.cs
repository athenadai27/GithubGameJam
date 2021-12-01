using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntScript : MonoBehaviour
{
    public enum AlertLevels { sleeping, backToStart, searching, pursuit, wakingUp, attacking, lured, performingAction, suspicious, waitForNextState };

    public AlertLevels alertLevel;

    public GameObject antText;
    public GameObject chillText;
    public GameObject notAnAntText;
    public TextControllerV2 textController;

    public Transform playerTransform;

   public GameObject currentTextController;

    

    [SerializeField]
    private CircleCollider2D playerDetector;
    [SerializeField]
    private LayerMask playerMask;

    
    public float delayTime;
    public bool waiting;
    public GameObject nextText;
   
    public Transform canvas;
    public LayerMask eventMask;
    public GameObject colliderToDisable;

    // Start is called before the first frame update
    void OnEnable()
    {

        alertLevel = AlertLevels.sleeping;
        ActivateTextController(chillText);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerDetectCollider = Physics2D.OverlapCircle(playerDetector.bounds.center, playerDetector.radius, playerMask);
        if (playerDetectCollider && alertLevel != AlertLevels.wakingUp && alertLevel != AlertLevels.pursuit && alertLevel != AlertLevels.attacking)
        {
            if(!playerDetectCollider.gameObject.GetComponentInChildren<StemController>().hasAntMask ){
                ActivateTextController(notAnAntText);
            } else if(playerDetectCollider.gameObject.GetComponentInChildren<StemController>().hasAntMask){
                alertLevel = AlertLevels.sleeping;
                ActivateTextController(antText);
                if(colliderToDisable != null){
                    colliderToDisable.SetActive(false);
                }
               
            }
            
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

        currentTextController.SetActive(true);
    }





}
