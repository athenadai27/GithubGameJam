using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WordBlock : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public float linearSpeed = 5.0f;
    public Vector3 destination;

    private bool arrived = false;

    public void PutWord(string word)
    {
        textBox.text = word;
    }

    public void SetDest(Vector3 destPos)
    {
        destination = destPos;
    }

    public void Launch(Vector3 launchingPoint)
    {
        gameObject.SetActive(true);
        transform.position = launchingPoint;
        arrived = false;
    }

    private void Update()
    {
        // word block should control its own movement behavior here
        if (!arrived)
        {
            Vector3 movement = destination - transform.position;

            if (movement.magnitude <= linearSpeed * Time.deltaTime)
            {
                transform.position = destination;
                arrived = true;
            }
            else
            {
                movement = movement.normalized * linearSpeed * Time.deltaTime;
                transform.position += movement;
            }
        }
    }
}
