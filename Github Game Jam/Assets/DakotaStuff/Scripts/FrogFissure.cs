using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogFissure : MonoBehaviour
{
    public float moveSpeed;
    public bool moveRight;
    public BoxCollider2D boxCollider;
    public LayerMask fissureMask;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (moveRight)
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        Collider2D fissureCollider = Physics2D.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.size, 0, fissureMask);
        if (fissureCollider)
        {
            if (fissureCollider.gameObject.name.Contains("Shield"))
            {
                gameObject.SetActive(false);
                fissureCollider.gameObject.GetComponent<ItemScript>().Break();
            }

        }
    }
}
