using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyToSomething : MonoBehaviour
{
    public KeyCode neededKey;
    public int mouseButton;
    public enum MouseButtonState {down, drag, up};
    public MouseButtonState mouseState;
    public bool keyNeeded;
    public bool conditionSatisfied;
    public float dieTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(conditionSatisfied){
            if(Time.time > dieTime){
                gameObject.SetActive(false);
            }
        } else{
            if(keyNeeded){
                if(Input.GetKeyDown(neededKey)){
                    ConditionSatisfied();
                }
            } else{
                switch(mouseState){
                    case MouseButtonState.down:
                        if(Input.GetMouseButtonDown(mouseButton)){
                           ConditionSatisfied(); 
                        }
                        break;
                    case MouseButtonState.drag:
                        if(Input.GetMouseButton(mouseButton)){
                           ConditionSatisfied(); 
                        }
                        break;
                    case MouseButtonState.up:
                        if(Input.GetMouseButtonUp(mouseButton)){
                           ConditionSatisfied(); 
                        }
                        break;
                }
            }
        }
    }

    public void ConditionSatisfied(){
        conditionSatisfied = true;
        dieTime = Time.time + 1f;
    }
}
