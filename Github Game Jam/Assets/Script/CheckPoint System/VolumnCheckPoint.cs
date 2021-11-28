using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumnCheckPoint : CheckPoint
{ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.gameObject.tag.Contains("Player"))
            return;

        Save();
    }
}
