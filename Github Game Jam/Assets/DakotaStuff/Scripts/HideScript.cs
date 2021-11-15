using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideScript : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider2D bodyCollider;
    [SerializeField]
    private LayerMask hideMask;
    [SerializeField]
    private GameObject lightHolder;
    [SerializeField]
    private bool hidden;
    [SerializeField]
    private Vector3 playerGoToPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hidden)
            {
                Appear();
            }
            else
            {
                Collider2D bodyOverlap = Physics2D.OverlapCapsule(bodyCollider.bounds.center, bodyCollider.bounds.size, bodyCollider.direction, 0, hideMask);

                if (bodyOverlap)
                {
                    Hide(bodyOverlap.gameObject);
                }
            }


        }

    }

    public void Hide(GameObject hideObject)
    {
        transform.position = hideObject.transform.position;
        lightHolder.SetActive(false);
        hidden = true;
    }

    public void Appear()
    {
        transform.position = playerGoToPosition;
        lightHolder.SetActive(true);
        hidden = false;
    }
}
