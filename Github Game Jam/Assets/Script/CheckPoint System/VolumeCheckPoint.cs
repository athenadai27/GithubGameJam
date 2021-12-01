using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumeCheckPoint : CheckPoint
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.gameObject.CompareTag("Player")) || activeCheckPoint == this)
            return;

        Save();
    }
}
