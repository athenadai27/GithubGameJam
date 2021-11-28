using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachingHowToGrabNPC : MonoBehaviour
{
    public TextControllerV2 textController;
    public GameObject wordDialogueController;
    public GameObject itemDialogueController;
    public GameObject dragPrompt;
    public StemController stemController;
    public enum HelpStates { waitingForWordGrab, waitingForItem }
    public HelpStates helpState;
    public GameObject retractPrompt;
    public GameObject giveToHoujeilahObject;
    public GameObject waitForTextObject;

    // Start is called before the first frame update
    void Start()
    {
        if(textController != null){
            textController.gameObject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (helpState)
        {
            case HelpStates.waitingForWordGrab:
                if (stemController.grabbedWord)
                {
                    if(wordDialogueController != null){
                        wordDialogueController.SetActive(true);
                    }
                    
                    if(dragPrompt != null){
                        dragPrompt.SetActive(false);
                    }
                    
                    helpState = HelpStates.waitingForItem;
                    //this.enabled = false;
                }
                break;
            case HelpStates.waitingForItem:
                if (stemController.grabbedItem)
                {
                    if(itemDialogueController != null){
                        itemDialogueController.SetActive(true);
                    }
                    if(retractPrompt != null){
                        retractPrompt.SetActive(false);
                    }
                    if(giveToHoujeilahObject != null){
                        giveToHoujeilahObject.SetActive(true);
                    }
                    if(waitForTextObject != null){
                        waitForTextObject.SetActive(true);
                    }
                    
                    this.enabled = false;
                }
                break;
        }

    }
}
