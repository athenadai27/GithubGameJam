using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject key;
    public BoxCollider2D doorCollider;
    public LayerMask playerMask;
    public string objectNeededName;
    public Transform flowerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(transform.position,doorCollider.bounds.size,0,playerMask);
        if(hitCollider){
           if(flowerTransform.childCount > 1){
               if(flowerTransform.GetChild(1).name.Contains(objectNeededName)){
                   gameObject.SetActive(false);
               }
           }
        }
    }
}
