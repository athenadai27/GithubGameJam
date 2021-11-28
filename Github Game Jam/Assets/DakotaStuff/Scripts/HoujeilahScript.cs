﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoujeilahScript : MonoBehaviour
{
    public List<GameObject> houjeilahDialogue;
    public List<GameObject> prompts;
    public enum HoujeilahStates {travelling,waiting}
    public HoujeilahStates houjeilahState;
    public float moveSpeed;
    public Vector3 startPos;
    public Vector3 travelPos;
    public float moveLerp;
    public GameObject houjeilahCanvas;
    public SpriteRenderer spriteRenderer;
    public Vector3 previousPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(houjeilahState){
            case HoujeilahStates.travelling:
                moveLerp += Time.deltaTime*moveSpeed;
                Vector3 nextPos = Vector3.Lerp(startPos,travelPos,moveLerp);
                if(nextPos.x >  transform.position.x){
                    transform.localScale = new Vector3(-1,1,1);
                    
                } else if(nextPos.x < transform.position.x){
                    transform.localScale = Vector3.one;
                }
                houjeilahCanvas.transform.localScale = transform.localScale;
                transform.position = nextPos;
                
                if(moveLerp >= 1){
                    houjeilahState = HoujeilahStates.waiting;
                }
                break;
            case HoujeilahStates.waiting:
                break;
        }
    }

    public void ActivateTextController(int textControllerIndex){
        houjeilahDialogue[textControllerIndex].SetActive(true);

    }

    public void ActivatePrompt(int promptIndex){
        prompts[promptIndex].SetActive(true);
    }

    public void GoToPosition(Vector3 goToPos){
        moveLerp = 0f;
        startPos = transform.position;
        travelPos = goToPos;
        houjeilahState = HoujeilahStates.travelling;
    }

    public void Hide(Vector3 hidePosition){
        previousPosition = transform.position;
        Color newColor = spriteRenderer.color;
        newColor.a = .5f;
        spriteRenderer.color = newColor;
        Debug.Log("houjeilahhide");
    }
    public void Appear(){
        transform.position = previousPosition;
        Color newColor = spriteRenderer.color;
        newColor.a = 1f;
        spriteRenderer.color = newColor;
        Debug.Log("houjeilahappear");
    }
}
