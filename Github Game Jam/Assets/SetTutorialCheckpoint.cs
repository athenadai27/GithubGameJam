using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTutorialCheckpoint : MonoBehaviour
{
    public BoxCollider2D checkpointCollider;
    public ActivateDeactivateTutorial activateDeactivate;
    public LayerMask playerMask;

    public List<Transform> entityTransforms;
    public enum CheckpointStates {activated, inactive};
    public CheckpointStates checkpointState = CheckpointStates.inactive;
    public PlayerController playerController;
    public CheckpointManager checkpointManager;
    public List<EnemyHealth> enemies;
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;
    public List<Vector3> entityPositions;
    public List<ActivateDeactivateTutorial> activateDeactivateScripts;
    public List<ActivateDeactivateTutorial> resetActivateDeactivateScripts;
    public List<TextControllerV2> textControllers;
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
                    for(int i = 0; i < entityTransforms.Count;i++){
                        entityPositions[i] = entityTransforms[i].position;
                    }
                    checkpointManager.currentCheckpoint = this;
                    checkpointState = CheckpointStates.activated;
                }
                break;
        }
    }

    public void ReturnToCheckpoint(){
        ItemScript[] items = FindObjectsOfType<ItemScript>();
        for(int i = 0; i < items.Length;i++){
            Destroy(items[i].gameObject);
        }
        GameObject[] extraObjects = GameObject.FindGameObjectsWithTag("Extra");
        for(int i = 0; i < extraObjects.Length;i++){
            Destroy(extraObjects[i]);
        }

        for(int i = 0; i < entityTransforms.Count;i++){
            entityTransforms[i].position = entityPositions[i];
        }
        for(int i = 0; i < enemies.Count;i++){
            enemies[i].Reset();
        }
        for(int i = 0; i < activateDeactivateScripts.Count;i++){
            activateDeactivateScripts[i].hasBeenActivated = false;
        }
        for(int i = 0; i <resetActivateDeactivateScripts.Count;i++){
           resetActivateDeactivateScripts[i].Reset();
        }
        for(int i = 0; i < textControllers.Count;i++){
            textControllers[i].Reset();
        }
       
       if(activateDeactivate != null){
           activateDeactivate.DeactivateObjects(objectsToDeactivate);
         activateDeactivate.ActivateObjects(objectsToActivate);
       }
        checkpointState = CheckpointStates.inactive;
       
    }
}
