using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    float talkRadius = 2.0f;
    
    Rigidbody2D rb;
    Collider2D cd;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += new Vector2(0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement -= new Vector2(0, 1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement -= new Vector2(1, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += new Vector2(1, 0);
        }

        if (movement != Vector2.zero)
        {
            movement = movement.normalized;
        }

        // rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        rb.velocity = movement * speed;

        // interaction
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, talkRadius);

            Collider2D closestHit = null;
            NonPlayer closesNpc = null;
            foreach (var hit in hits)
            {
                var tempNpc = hit.GetComponent<NonPlayer>();
                if (tempNpc && (!closesNpc || (hit.transform.position - transform.position).magnitude < (closestHit.transform.position - transform.position).magnitude))
                {
                    closesNpc = tempNpc;
                }
            }

            if (closesNpc)
                closesNpc.TalkTo();
        }
    }
}
