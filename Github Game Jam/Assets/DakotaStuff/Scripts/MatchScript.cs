using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScript : MonoBehaviour
{
    public BoxCollider2D matchCollider;
    public BoxCollider2D fireCollider;
    public LayerMask burnMask;
    public ItemScript itemScript;
    public Transform fireTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)){

            Collider2D matchTouch = Physics2D.OverlapBox(matchCollider.bounds.center,matchCollider.bounds.size,0,burnMask);
        if(matchTouch){
            if(matchTouch.gameObject.CompareTag("Toxic")){
                fireTransform.SetParent(matchTouch.transform);
                fireTransform.position = matchTouch.transform.position;
                matchTouch.GetComponent<PuddleScript>().shrinkSpeed *= 2;
                itemScript.Break();
                return;
            } else if(matchTouch.gameObject.GetComponent<BurnAlert>()){
                fireTransform.SetParent(matchTouch.transform);
                fireTransform.position = matchTouch.transform.position;

                matchTouch.gameObject.GetComponent<BurnAlert>().Burn();
                itemScript.Break();
            }
        }
        Collider2D fireTouch = Physics2D.OverlapBox(fireCollider.bounds.center,fireCollider.bounds.size,0,burnMask);
        if(fireTouch){
            if(fireTouch.gameObject.CompareTag("Toxic")){
                fireTransform.SetParent(fireTouch.transform);
                fireTransform.position = fireTouch.transform.position;
                fireTouch.GetComponent<PuddleScript>().shrinkSpeed *= 2;
                 itemScript.Break();
            }else if(fireTouch.gameObject.GetComponent<BurnAlert>()){
                fireTransform.SetParent(fireTouch.transform);
                fireTransform.position = fireTouch.transform.position;
                fireTouch.gameObject.GetComponent<BurnAlert>().Burn();
                itemScript.Break();
            }
        }
        }
        
    }
}
