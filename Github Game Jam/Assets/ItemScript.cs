using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public enum ItemStates {held,dropped,grounded};
    public ItemStates itemState;
    public BoxCollider2D itemCollider;
    public LayerMask groundMask;
    public float currentGravity;
    public GameObject poof;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(itemState){
            case ItemStates.held:
                break;
            case ItemStates.dropped:
                currentGravity += Time.deltaTime*9.8f;
                Vector3 moveVector = Vector3.up*Physics2D.gravity.y - Vector3.up*currentGravity;
                RaycastHit2D groundCast = Physics2D.BoxCast(itemCollider.bounds.center, itemCollider.bounds.size,0,Vector2.down,moveVector.magnitude*Time.deltaTime,groundMask);
                if(groundCast){
                    transform.position = (Vector3)groundCast.point + itemCollider.bounds.extents.y*Vector3.up - (Vector3)itemCollider.offset;
                    itemState = ItemStates.grounded;
                } else{
                    transform.position += moveVector*Time.deltaTime;
                }
                break;
            case ItemStates.grounded:
                break;
        }
    }

    public void Drop(){
        currentGravity = 9.8f;
        itemState = ItemStates.dropped;
        transform.SetParent(null);
        if(!itemCollider.enabled){
            itemCollider.enabled = true;
        }
    }

    public void Break(){
        Instantiate(poof,transform.position,poof.transform.rotation);
        Destroy(this.gameObject);
    }
}
