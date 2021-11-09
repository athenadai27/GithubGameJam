using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WordGrabber : MonoBehaviour
{
    public GameObject flower;
    public GraphicRaycaster graphicRaycaster;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
           PointerEventData pointerData = new PointerEventData(EventSystem.current);
 
             pointerData.position = Camera.main.WorldToScreenPoint(flower.transform.position);

             List<RaycastResult> results = new List<RaycastResult>();
             graphicRaycaster.Raycast(pointerData, results);
             
            if (results.Count > 0)
            {
                results[0].gameObject.GetComponent<GrabbableWord>().GrabItem(flower.transform);
            }
        

    }
}
