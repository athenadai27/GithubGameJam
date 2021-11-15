using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WordGrabber : MonoBehaviour
{
    public GameObject flower;
    public GraphicRaycaster graphicRaycaster;
    public GameObject playerCanvas;
    public List<GraphicRaycaster> graphicRaycasters;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Camera.main.WorldToScreenPoint(flower.transform.position);
        for (int i = 0; i < graphicRaycasters.Count; i++)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycasters[i].Raycast(pointerData, results);

            if (results.Count > 0)
            {
                if(results[0].gameObject.GetComponent<GrabbableWord>()){
                     results[0].gameObject.GetComponent<GrabbableWord>().GrabItem(playerCanvas.transform);
                }
               
            }
        }
        //  List<RaycastResult> results = new List<RaycastResult>();
        //  graphicRaycaster.Raycast(pointerData, results);

        // if (results.Count > 0)
        // {
        //     results[0].gameObject.GetComponent<GrabbableWord>().GrabItem(playerCanvas.transform);
        // }


    }
}
