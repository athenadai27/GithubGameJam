using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public SetTutorialCheckpoint currentCheckpoint;
    public PlayerController playerController;
    public HoujeilahScript houjeilah;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCheckpoint(){
        currentCheckpoint.ReturnToCheckpoint();
        playerController.Reset();
        if(houjeilah != null){
            houjeilah.Reset();
        }
    }
}
