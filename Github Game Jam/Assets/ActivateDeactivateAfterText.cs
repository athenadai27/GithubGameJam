using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateAfterText : MonoBehaviour
{
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;
    public TextControllerV2 textController;
    public List<TextControllerV2> textControllersActivate;
    public List<TextControllerV2> textControllersDeactivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateAndDeactivate(){
        for(int i = 0; i < objectsToActivate.Count;i++){
            objectsToActivate[i].SetActive(true);
        }
        for(int i = 0; i < objectsToDeactivate.Count;i++){
            objectsToDeactivate[i].SetActive(false);
        }
        for(int i = 0; i < textControllersActivate.Count;i++){
            textControllersActivate[i].gameObject.SetActive(true);
        }
        for(int i = 0; i < textControllersDeactivate.Count;i++){
            textControllersDeactivate[i].FadeText();
        }
    }
}
