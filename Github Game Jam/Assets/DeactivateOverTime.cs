using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOverTime : MonoBehaviour
{
    public float deactivateTime;
    public float deactivateTimeAdditional;
    // Start is called before the first frame update
    void OnEnable()
    {
        deactivateTime = Time.time + deactivateTimeAdditional;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > deactivateTime){
            gameObject.SetActive(false);
        }
    }
}
