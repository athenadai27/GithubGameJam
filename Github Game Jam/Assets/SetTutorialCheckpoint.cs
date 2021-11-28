using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTutorialCheckpoint : MonoBehaviour
{
    public BoxCollider2D checkpointCollider;
    public ActivateDeactivateAfterText activateDeactivateAfterText;
    public LayerMask playerMask;

    public List<Transform> entityTransforms;
    public List<Transform> newPositionTransforms;
    public enum CheckpointStates {activated, inactive};
    public CheckpointStates checkpointState = CheckpointStates.inactive;
    public PlayerController playerController;
    public CheckpointManager checkpointManager;
    // Start is called before the first frame update
    void OnEnable()
    {
        checkpointState = CheckpointStates.inactive;
    }

    // Update is called once per frame
    void Update()
    {
        switch(checkpointState){
            case CheckpointStates.activated:
                break;
            case CheckpointStates.inactive:
                Collider2D checkpointOverlap = Physics2D.OverlapBox(checkpointCollider.bounds.center,checkpointCollider.bounds.size,0,playerMask);
                if(checkpointOverlap){
                    checkpointManager.currentCheckpoint = this;
                }
                break;
        }
    }

    public void ReturnToCheckpoint(){
        for(int i = 0; i < entityTransforms.Count;i++){
            entityTransforms[i].position = newPositionTransforms[i].position;
        }
        activateDeactivateAfterText.ActivateAndDeactivate();
    }
}
