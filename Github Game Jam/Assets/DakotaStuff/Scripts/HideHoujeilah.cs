using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideHoujeilah : MonoBehaviour
{
    public BoxCollider2D hideCollider;
    public LayerMask houjeilahMask;
    public Transform hideTransform;
    public Transform appearTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D houjeilahOverlap = Physics2D.OverlapBox(hideCollider.bounds.center,hideCollider.bounds.size,0f,houjeilahMask);
        if(houjeilahOverlap){
            houjeilahOverlap.GetComponent<HoujeilahScript>().Hide(hideTransform.position);
            this.enabled = false;
        }
    }
}
