using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskFallScript : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public Transform targetTransform;
    public float moveLerp;
    public float secondsToArrive;
    public StemController stemController;
    public GameObject antMask;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = targetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveLerp += Time.deltaTime/secondsToArrive;
        transform.position = Vector3.Lerp(startPos,endPos,moveLerp);
        if(moveLerp >= 1){
            antMask.SetActive(true);
            stemController.hasAntMask = true;
            stemController.frogMask.SetActive(false);
            gameObject.SetActive(false);
        }
        
    }
}
