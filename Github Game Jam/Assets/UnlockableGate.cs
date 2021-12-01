using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableGate : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public LayerMask keyMask;
    public HoujeilahScript houjeilah;
    public Transform nextHoujeilahPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D keyOverlap = Physics2D.OverlapBox(boxCollider.bounds.center,boxCollider.bounds.size,0f,keyMask);
        if(keyOverlap){
            if(keyOverlap.name.Contains("Key")){
                keyOverlap.GetComponent<ItemScript>().Break();
                houjeilah.GoToPosition(nextHoujeilahPosition.position,0f);
                gameObject.SetActive(false);
                
            }
        }
    }
}
