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
    public StemController stemController;
    public CircleCollider2D flowerCollider;
    public LayerMask itemMask;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (stemController.playerState == StemController.PlayerStates.drawing)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Camera.main.WorldToScreenPoint(flower.transform.position);
            for (int i = 0; i < graphicRaycasters.Count; i++)
            {
                List<RaycastResult> results = new List<RaycastResult>();
                graphicRaycasters[i].Raycast(pointerData, results);

                if (results.Count > 0)
                {
                    if (results[0].gameObject.GetComponent<GrabbableWord>() && stemController.grabbedWord == null && stemController.grabbedItem == null)
                    {
                        results[0].gameObject.GetComponent<GrabbableWord>().GrabItem(playerCanvas.transform);
                    }

                }
            }
            Collider2D flowerOverlap = Physics2D.OverlapCircle(flowerCollider.bounds.center, flowerCollider.radius, itemMask);
            if (flowerOverlap)
            {
                if (stemController.grabbedWord == null && stemController.grabbedItem == null && flowerOverlap.gameObject.GetComponent<ItemScript>().itemState == ItemScript.ItemStates.grounded){
                   
                    stemController.grabbedItem = flowerOverlap.gameObject;
                    flowerOverlap.transform.position = flower.transform.position;
                    flowerOverlap.transform.SetParent(flower.transform);
                     if(flowerOverlap.gameObject.name.Contains("Sword")){
                        flowerOverlap.enabled = false;
                    }
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
