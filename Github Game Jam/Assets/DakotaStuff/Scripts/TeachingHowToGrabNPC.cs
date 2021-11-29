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
    public List<GameObject> objectsToActivateFromWord;
    public List<GameObject> objectsToDeactivateFromWord;
     public List<GameObject> objectsToActivateFromItem;
    public List<GameObject> objectsToDeactivateFromItem;
    public List<TextControllerV2> textControllersToDeactivateFromWord;
    public List<TextControllerV2> textControllersToDeactivateFromItem;
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

                    for(int i = 0; i < objectsToActivateFromWord.Count;i++){
                        objectsToActivateFromWord[i].SetActive(true);
                    }
                    for(int i = 0; i < objectsToDeactivateFromWord.Count;i++){
                        objectsToDeactivateFromWord[i].SetActive(false);
                    }
                    for(int i = 0; i < textControllersToDeactivateFromWord.Count;i++){
                        textControllersToDeactivateFromWord[i].FadeText();
                    }
                    helpState = HelpStates.waitingForItem;
                }
                break;
            case HelpStates.waitingForItem:
                if (stemController.grabbedItem)
                {

                    for(int i = 0; i < objectsToActivateFromItem.Count;i++){
                        objectsToActivateFromItem[i].SetActive(true);
                    }
                    for(int i = 0; i < objectsToDeactivateFromItem.Count;i++){
                        objectsToDeactivateFromItem[i].SetActive(false);
                    }
                    for(int i = 0; i < textControllersToDeactivateFromItem.Count;i++){
                        textControllersToDeactivateFromItem[i].FadeText();
                    }
                    gameObject.SetActive(false);
                }
                break;
        }

    }
}
