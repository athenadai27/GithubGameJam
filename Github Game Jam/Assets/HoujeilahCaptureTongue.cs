using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoujeilahCaptureTongue : MonoBehaviour
{
    public LineRenderer tongue;
    public Transform target;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 targetPos;
    public float lerp;
    public float tongueSpeed;
    public float distanceFromEndStart;
    public float outDistanceFromEndStart;
    public LayerMask playerMask;
    public bool capturedPlayer;
    public Transform houjeilahTransform;

    public enum TongueStates { goingOut, goingIn,waitingForHoujeilah, dormant };
    public TongueStates tongueState = TongueStates.dormant;

    public bool attacking;
    public bool waitingForHoujeilahText;
    public GameObject houjeilahText;
    public GameObject fakeSceneTransition;
    // Start is called before the first frame update
    void OnEnable()
    {
        ActivateTongue();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (tongueState)
        {
            case TongueStates.dormant:
                break;
            case TongueStates.goingIn:
                lerp += Time.deltaTime * tongueSpeed * 2f;
                targetPos += (tongue.transform.position - targetPos).normalized * tongueSpeed * Time.deltaTime;
                // targetPos = Vector3.Lerp(endPos, tongue.transform.position, lerp);
                if (capturedPlayer)
                {
                    houjeilahTransform.position = targetPos;
                }
                float distanceToTarget = Vector3.Distance(tongue.transform.position, targetPos);
                float segmentDistance = distanceToTarget / tongue.positionCount;
                Vector3 targetDirection = (targetPos - tongue.transform.position).normalized;
                float distanceFromEnd = Vector3.Distance(targetPos, endPos);
                for (int i = 0; i < tongue.positionCount; i++)
                {

                    tongue.SetPosition(i, tongue.transform.position + targetDirection * (i * segmentDistance) + Vector3.up * Mathf.Sin(i * segmentDistance) * Mathf.Sin(1 - (distanceFromEndStart - distanceFromEnd) / distanceFromEndStart));
                }
                if (distanceFromEnd >= Vector3.Distance(tongue.transform.position, endPos))
                {
                    fakeSceneTransition.SetActive(true);
                    tongue.gameObject.SetActive(false);
                    
                    if (capturedPlayer)
                    {
                        capturedPlayer = false;
                    }

                    tongueState = TongueStates.dormant;
                    attacking = false;
                }
                break;
            case TongueStates.goingOut:
                lerp += Time.deltaTime * tongueSpeed;
                targetPos += (endPos - targetPos).normalized * tongueSpeed * Time.deltaTime;
                //targetPos = Vector3.Lerp(tongue.transform.position, endPos, lerp);
                float outDistanceToTarget = Vector3.Distance(tongue.transform.position, targetPos);
                float outSegmentDistance = outDistanceToTarget / tongue.positionCount;
                Vector3 outTargetDirection = (targetPos - tongue.transform.position).normalized;
                float outDistanceFromEnd = Vector3.Distance(targetPos, endPos);
                Vector2[] tonguePoints = new Vector2[tongue.positionCount];
                for (int i = 0; i < tongue.positionCount; i++)
                {

                    tongue.SetPosition(i, tongue.transform.position + outTargetDirection * (i * outSegmentDistance) + Vector3.up * Mathf.Sin(i * outSegmentDistance) * Mathf.Sin(1 - (outDistanceFromEndStart - outDistanceFromEnd) / outDistanceFromEndStart));
                    tonguePoints[i] = (Vector2)tongue.GetPosition(i) - (Vector2)tongue.transform.position;
                }



                if (outDistanceToTarget >= Vector3.Distance(tongue.transform.position, endPos))
                {
                    lerp = 0;
                    capturedPlayer = true;
                    tongueState = TongueStates.waitingForHoujeilah;
                    houjeilahText.SetActive(true);
                }
                break;
            case TongueStates.waitingForHoujeilah:
                break;
        }
        if (!capturedPlayer)
        {


            // tongue.SetPosition(0,tongue.transform.position);
            // tongue.SetPosition(1,target.position);
        }
        else
        {

        }

    }

    public void ActivateTongue()
    {

        endPos = houjeilahTransform.position + Vector3.up;
        outDistanceFromEndStart = Vector3.Distance(endPos, tongue.transform.position);
        distanceFromEndStart = outDistanceFromEndStart;
        tongueState = TongueStates.goingOut;
        lerp = 0;
        targetPos = tongue.transform.position;
        attacking = true;
        for (int i = 0; i < tongue.positionCount; i++)
        {
            tongue.SetPosition(i, tongue.transform.position);
        }
        tongue.gameObject.SetActive(true);
    }

    public void Retract()
    {
        tongueState = TongueStates.goingIn;
    }

    public void Reset()
    {
        Debug.Log("reset");
        tongueState = TongueStates.dormant;
        tongue.gameObject.SetActive(false);
        capturedPlayer = false;
    }
}
