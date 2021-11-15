using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class LightController : MonoBehaviour
{
    [SerializeField]
    private Light2D outerLight;
    [SerializeField]
    private Light2D middleLight;
    [SerializeField]
    private Light2D innerLight;
    [SerializeField]
    private CircleCollider2D outerLightCollider;
    [SerializeField]
    private CircleCollider2D middleLightCollider;
    [SerializeField]
    private CircleCollider2D innerLightCollider;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private enum LightState {light,delight};
    private LightState lightState;
    [SerializeField]
    private float lightLerp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.moveDir != 0){
            ChangeLight(Time.deltaTime);
        } else{
            ChangeLight(-Time.deltaTime);
        }
    }

    public void ChangeLight(float lightIncrease){
        lightLerp += lightIncrease;
        if(lightLerp > 1){
            lightLerp = 1;
        } else if(lightLerp < 0){
            lightLerp = 0;
        }
        outerLightCollider.radius = Mathf.Lerp(5,7,lightLerp);
        middleLightCollider.radius = Mathf.Lerp(3,5,lightLerp);
        innerLightCollider.radius =  Mathf.Lerp(1,3,lightLerp);
        outerLight.pointLightOuterRadius = Mathf.Lerp(5,7,lightLerp);
        middleLight.pointLightOuterRadius = Mathf.Lerp(3,5,lightLerp);
        innerLight.pointLightOuterRadius =  Mathf.Lerp(1,3,lightLerp);
    }

}
