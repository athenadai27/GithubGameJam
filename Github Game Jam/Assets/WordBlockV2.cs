using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WordBlockV2 : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public float linearSpeed = 5.0f;
    public Vector3 destination;

    public float refHeightRandomizationFactor = 0;

    public float amplitude = 1.0f;
    public float amplitudeRandomizeFactor = 0.2f;
    public float speedFactor = 1.0f;

    public bool arrived = false;
    private float phase = 0;
    private float activeAmplitude = 0;

    public void PutWord(string word)
    {
        textBox.text = word;
    }

    public float CalculateWidthEstimate()
    {
        float shorter = textBox.preferredWidth;
        string word = textBox.text;
        textBox.text += " " + word;
        float longer = textBox.preferredWidth;

        textBox.text = word;
        return (longer - shorter) * transform.localScale.x;
    }

    public void SetDest(Vector3 destPos)
    {
        destination = destPos;
        destination.y += Random.Range(-refHeightRandomizationFactor, refHeightRandomizationFactor);
    }

    public void Launch(Vector3 launchingPoint)
    {
        gameObject.SetActive(true);
        transform.localPosition = launchingPoint;
        arrived = false;

        // randomize
        float amplitudeMultiplier = 1 + Random.Range(-amplitudeRandomizeFactor, amplitudeRandomizeFactor);
        activeAmplitude = amplitude * amplitudeMultiplier;
    }

    private void Update()
    {
        // word block should control its own movement behavior here
        if (!arrived)
        {
            Vector3 movement = destination - transform.localPosition;

            if (movement.magnitude <= linearSpeed * Time.deltaTime)
            {
                transform.localPosition = destination;
                arrived = true;
                Debug.Log("arrived");
                Debug.Log(movement.magnitude);
                phase = 0;
            }
            else
            {
                movement = movement.normalized * linearSpeed * Time.deltaTime;
                transform.localPosition += movement;
            }
        }
        else
        {
            Vector3 cachedPosition = transform.localPosition;
            cachedPosition.y = destination.y + Mathf.Sin(phase) * activeAmplitude;
            transform.localPosition = cachedPosition;

            phase += Time.deltaTime * speedFactor;
        }
    }
}
