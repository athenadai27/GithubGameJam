using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float followDelay = 0.2f;

    GameObject player;

    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 newPos = player.transform.position;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }

    void Update()
    {
        var tempTarget = player.transform.position;
        tempTarget.z = transform.position.z;

        var tempPath = Vector3.SmoothDamp(transform.position, tempTarget, ref velocity, followDelay);
        transform.position = tempPath;
    }
}
