using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public LayerMask burnableMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            Collider2D fireOverlap = Physics2D.OverlapCircle(transform.position,1f,burnableMask);
            if(fireOverlap){
                fireOverlap.GetComponent<BurnAlert>().Burn();
                transform.SetParent(fireOverlap.transform);
            }
        }
    }
}
