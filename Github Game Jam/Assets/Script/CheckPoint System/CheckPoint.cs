using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Parent class of check point types
/// </summary>
public class CheckPoint : MonoBehaviour
{
    static protected CheckPoint activeCheckPoint = null;

    [SerializeField]
    protected UnityEvent beforeTriggering;
    [SerializeField]
    protected UnityEvent afterTriggering;

    [SerializeField]
    protected UnityEvent onLoad;

    PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

    private Vector3 savedPosition;

    static public void LoadCheckPoint()
    {
        if (activeCheckPoint)
            activeCheckPoint.Load();
    }

    protected virtual void Load()
    {
        // restore player controller
        player.Reset();

        // seems to reset the flower?
        ItemScript[] items = FindObjectsOfType<ItemScript>();
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Break();
        }

        // reset position
        player.transform.position = savedPosition;

        onLoad.Invoke();
    }

    protected void Save()
    {
        beforeTriggering.Invoke();

        savedPosition = player.transform.position;
        activeCheckPoint = this;

        afterTriggering.Invoke();
    }

    private void OnDestroy()
    {
        activeCheckPoint = null;
    }
}
