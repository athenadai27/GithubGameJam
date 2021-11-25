using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> dialogues;

    [SerializeField]
    private List<float> timeBetweenDialogues;

    private float currentTimeBetweenDialogues;

    private int currentDialogueIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentTimeBetweenDialogues = Time.time + timeBetweenDialogues[currentDialogueIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if(currentDialogueIndex < dialogues.Count){
            if(Time.time > currentTimeBetweenDialogues){
            dialogues[currentDialogueIndex].SetActive(true);
            currentDialogueIndex++;
            if(currentDialogueIndex < dialogues.Count){
                currentTimeBetweenDialogues = Time.time + timeBetweenDialogues[currentDialogueIndex];
            }
            
        }
        }
        
    }
}
