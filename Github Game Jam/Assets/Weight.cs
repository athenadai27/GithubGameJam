using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    public LayerMask fallMask;
    public BoxCollider2D weightCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D weightOverlap = Physics2D.OverlapBox(weightCollider.bounds.center,weightCollider.bounds.size,0,fallMask);
        if(weightOverlap){
            
        }
    }
}
