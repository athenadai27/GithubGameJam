using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownHoujeilah : MonoBehaviour
{
    public TextControllerV2 textController;
    public float newHoujeilahSpeed;
    public HoujeilahScript houjeilah;
    // Start is called before the first frame update
    void OnDisable(){
        Debug.Log("deactivated");
    }

    void Update(){
        if(textController.CheckIfArrived()){
            houjeilah.moveSpeed = newHoujeilahSpeed;
        }
    }
}
