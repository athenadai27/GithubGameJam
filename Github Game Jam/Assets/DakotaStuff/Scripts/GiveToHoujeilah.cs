using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveToHoujeilah : MonoBehaviour
{
    public StemController stemController;
    public TextControllerV2 textController;
    public bool waiting;
    public float delayTime;
    public Transform houjeilahArmTransform;
    public GameObject nextDialoguePrompt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(!textController.gameObject.activeSelf){
           stemController.grabbedItem.transform.SetParent(houjeilahArmTransform);
           stemController.grabbedItem.transform.position = houjeilahArmTransform.position;
           stemController.grabbedItem.GetComponent<BoxCollider2D>().enabled = true;
           stemController.grabbedItem = null;
           nextDialoguePrompt.SetActive(true);

           gameObject.SetActive(false);
       }
    }
}
