using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public BoxCollider2D dialogueCollider;
    public BoxCollider2D disablePromptCollider;
    public LayerMask playerMask;
    public enum DialogueState {inactive, activated, done};
    public DialogueState dialogueState;
    public TextControllerV2 textController;
    public GameObject prompt;
    public bool waiting;
    public float afterTextDelayTime;
    public float afterTextDelayTimeAdditional;
    public HoujeilahScript houjeilah;
    public Transform houjeilahTravelPosition;
    public PlayerController playerController;
    public bool travel;
    public float jumpHeight;
    // Start is called before the first frame update
    void OnEnable()
    {
        dialogueState = DialogueState.inactive;
    }

    // Update is called once per frame
    void Update()
    {
        switch(dialogueState){
            case DialogueState.inactive:
                Collider2D playerCollide = Physics2D.OverlapBox(dialogueCollider.bounds.center,dialogueCollider.bounds.size,0,playerMask);
                if(playerCollide){
                    dialogueState = DialogueState.activated;
                    playerController.canMove = false;
                    textController.gameObject.SetActive(true);
                    waiting = false;
                }
                break;
            case DialogueState.activated:
                if(waiting && Time.time > afterTextDelayTime){       
                    playerController.canMove = true;
                    textController.FadeText();
                    dialogueState = DialogueState.done;
                    if(travel){
                        houjeilah.GoToPosition(houjeilahTravelPosition.position,jumpHeight);
                    }
                    
                } else{
                    if(textController.CheckIfArrived() && !waiting){
                        waiting = true;
                        afterTextDelayTime = Time.time + afterTextDelayTimeAdditional;
                        
                    }
                }
                break;
            case DialogueState.done:
                Collider2D disablePromptCollide = Physics2D.OverlapBox(disablePromptCollider.bounds.center,disablePromptCollider.bounds.size,0,playerMask);
                if(disablePromptCollide){
                    prompt.SetActive(false);
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    
}
