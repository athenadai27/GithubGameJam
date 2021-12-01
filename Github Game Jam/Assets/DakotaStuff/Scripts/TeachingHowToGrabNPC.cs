using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachingHowToGrabNPC : ActivateDeactivateTutorial
{
    public TextControllerV2 textController;
    
    public StemController stemController;
    public enum HelpStates { waitingForWordGrab, waitingForItem }
    public HelpStates helpState;
    public List<GameObject> objectsToActivateFromWord;
    public List<GameObject> objectsToDeactivateFromWord;
     public List<GameObject> objectsToActivateFromItem;
    public List<GameObject> objectsToDeactivateFromItem;
    public List<TextControllerV2> textControllersToDeactivateFromWord;
    public List<TextControllerV2> textControllersToDeactivateFromItem;
    public bool done;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(textController != null){
            textController.gameObject.SetActive(true);
        }
        done = false;
        helpState = HelpStates.waitingForWordGrab;
    }

    // Update is called once per frame
    void Update()
    {
        switch (helpState)
        {
            case HelpStates.waitingForWordGrab:
                if (stemController.grabbedWord)
                {
                    ActivateObjects(objectsToActivateFromWord);
                    DeactivateObjects(objectsToDeactivateFromWord);

                    for(int i = 0; i < textControllersToDeactivateFromWord.Count;i++){
                        textControllersToDeactivateFromWord[i].FadeText();
                    }
                    helpState = HelpStates.waitingForItem;
                }
                break;
            case HelpStates.waitingForItem:
                if (stemController.grabbedItem && !done)
                {

                    ActivateObjects(objectsToActivateFromItem);
                    DeactivateObjects(objectsToDeactivateFromItem);
                    for(int i = 0; i < textControllersToDeactivateFromItem.Count;i++){
                        textControllersToDeactivateFromItem[i].FadeText();
                    }
                    done = true;
                }
                break;
        }

    }

}
