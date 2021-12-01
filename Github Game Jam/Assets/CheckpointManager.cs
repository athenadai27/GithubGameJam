using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public SetTutorialCheckpoint currentCheckpoint;
    public PlayerController playerController;
    public HoujeilahScript houjeilah;
    public FadeScreen fadeScreen;
    public bool waitingToRespawn;
    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingToRespawn){
            if(Time.time > waitTime){
                GoToCheckpoint();
            }
        }
    }


    public void StartRespawning(){
        fadeScreen.StartCoroutine(fadeScreen.FadeOutRoutine());
        waitingToRespawn = true;
        waitTime = Time.time + 2f;
    }
    public void GoToCheckpoint(){
        
        currentCheckpoint.ReturnToCheckpoint();
        playerController.Reset();
        
        
        if(houjeilah != null){
            houjeilah.Reset();
        }
        fadeScreen.StartCoroutine(fadeScreen.FadeInRoutine());
        waitingToRespawn = false;
    }
}
