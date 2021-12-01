using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateTutorial : MonoBehaviour
{

    public bool useOnEnable;
    public bool useOnDisable;
    public bool useOnCollision;
    public bool disableBoutheinaMoveOnCollision;
    public bool enableBoutheinaMoveOnEnable;
    public bool enableBoutheinaMoveOnDisable;
    public PlayerController playerController;
    public bool MoveHoujeilahOnDisable;
    public HoujeilahScript houjeilah;
    public Transform houjeilahMoveTransform;
    public List<GameObject> objectsToActivateOnEnable;
    public List<GameObject> objectsToDeactivateOnEnable;
    public List<GameObject> objectsToActivateOnDisable;
    public List<GameObject> objectsToDeactivateOnDisable;

    public List<GameObject> objectsToActivateOnCollision;
    public List<GameObject> objectsToDeactivateOnCollision;
    public List<TextControllerV2> textsToDisableOnEnable;
    public List<TextControllerV2> textsToDisableOnDisable;
    public List<GameObject> objectsToActivateWhenTextFinishedButNotDeactivated;
    public List<GameObject> objectsToDeactivateWhenTextFinishedButNotDeactivated;
    public BoxCollider2D boxCollider;
    public LayerMask playerMask;
    public bool hasBeenActivated;
    public float jumpHeight;
    public bool prematureDeactivation;
    public List<ActivateDeactivateTutorial> prematureDeactivations;
    void OnEnable()
    {
        if (useOnEnable)
        {
            if(enableBoutheinaMoveOnEnable){
                playerController.canMove = true;
            }
            ActivateObjects(objectsToActivateOnEnable);
            DeactivateObjects(objectsToDeactivateOnEnable);
            DeactivateTexts(textsToDisableOnEnable);
        }

    }

    void OnDisable()
    {
         if (useOnDisable && !prematureDeactivation)
        {
            if(enableBoutheinaMoveOnDisable){
                playerController.canMove = true;
            }
            if(MoveHoujeilahOnDisable){
                houjeilah.GoToPosition(houjeilahMoveTransform.position,jumpHeight);
            }
            
            ActivateObjects(objectsToActivateOnDisable);
            DeactivateObjects(objectsToDeactivateOnDisable);
            DeactivateTexts(textsToDisableOnDisable);
        }
    }
    
    void Update(){
        if(useOnCollision && !hasBeenActivated){
            
            if(Physics2D.OverlapBox(boxCollider.bounds.center,boxCollider.bounds.size,0f,playerMask)){
                if(disableBoutheinaMoveOnCollision){
                playerController.canMove = false;
            }
                hasBeenActivated = true;
                ActivateObjects(objectsToActivateOnCollision);
                DeactivateObjects(objectsToDeactivateOnCollision);
            }
        }
    }

    public void ActivateObjects(List<GameObject> activateObjects){

            for (int i = 0; i < activateObjects.Count; i++)
            {
                activateObjects[i].SetActive(true);
            }
    }

    public void DeactivateObjects(List<GameObject> deActivateObjects){

            for (int i = 0; i < deActivateObjects.Count; i++)
            {
                if(deActivateObjects[i].GetComponent<ActivateDeactivateTutorial>() != null){
                    if(prematureDeactivations.Contains(deActivateObjects[i].GetComponent<ActivateDeactivateTutorial>())){
                        deActivateObjects[i].GetComponent<ActivateDeactivateTutorial>().prematureDeactivation = true;
                    }
                }
                deActivateObjects[i].SetActive(false);
            }
    }

    public void DeactivateTexts(List<TextControllerV2> textControllers){
        for (int i = 0; i < textControllers.Count; i++)
            {
                textControllers[i].FadeText();
            }
    }


    public virtual void Reset(){
        prematureDeactivation = false;
    }


}
