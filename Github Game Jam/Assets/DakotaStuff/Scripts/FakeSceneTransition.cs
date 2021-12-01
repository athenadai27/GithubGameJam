using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSceneTransition : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public enum FakeSceneTransitionStates {fadingIn, fadingOut,waitingForFade, waitingForCollision};
    public float fadeWaitTime;
    public FakeSceneTransitionStates transitionState;
    public BoxCollider2D transitionCollider;
    public Transform teleportPos;
    public Transform cameraTransform;
    public SpriteRenderer backgroundSpriteRenderer;
    public Sprite newBackgroundSprite;
    public Transform playerTransform;
    public LayerMask playerMask;
    public FollowPlayer followPlayerScript;
    public BoxCollider2D newMapBounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(transitionState){
            case FakeSceneTransitionStates.waitingForCollision:
                Collider2D transitionOverlap = Physics2D.OverlapBox(transitionCollider.bounds.center,transitionCollider.bounds.size,0f,playerMask);
                if(transitionOverlap){
                    StartCoroutine(fadeScreen.FadeOutRoutine());
                    transitionState = FakeSceneTransitionStates.fadingOut;
                }
                break;
            case FakeSceneTransitionStates.fadingIn:
                if(fadeScreen.overlayImage.color.a <= 0){
                    transitionState = FakeSceneTransitionStates.waitingForCollision;
                    followPlayerScript.enabled = true;
                }  
                break;
            case FakeSceneTransitionStates.fadingOut:
                if(fadeScreen.overlayImage.color.a >= 1){
                    fadeWaitTime = Time.time + 1f;
                    transitionState = FakeSceneTransitionStates.waitingForFade;
                     followPlayerScript.enabled = false;
                }   
                break;
            case FakeSceneTransitionStates.waitingForFade:
                if(Time.time > fadeWaitTime){
                    
                    playerTransform.position = teleportPos.position;
                   
                     followPlayerScript.FocusPlayer(newMapBounds);
                     
                    backgroundSpriteRenderer.sprite = newBackgroundSprite;
                    StartCoroutine(fadeScreen.FadeInRoutine());
                    transitionState = FakeSceneTransitionStates.fadingIn;
                }
                break;
        }
    }
}
