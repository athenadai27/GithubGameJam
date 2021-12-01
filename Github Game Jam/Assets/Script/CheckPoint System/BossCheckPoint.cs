using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCheckPoint : CheckPoint
{
    private int savedPhaseNumber = 0;
    private Vector3 savedPosition;
    private AttachedBoss attachedBoss;

    private enum AttachedBoss
    {
        GARBARA,
        FROG_KING
    }

    private void Start()
    {
        if (gameObject.GetComponent<FrogKingScript>())
        {
            attachedBoss = AttachedBoss.FROG_KING;
        }
        else
        {
            attachedBoss = AttachedBoss.GARBARA;
        }
    }

    public void SaveBoss(int phase)
    {
        savedPhaseNumber = phase;
    }

    protected override void Load()
    {
        base.Load();

        switch (attachedBoss)
        {
            case AttachedBoss.GARBARA:
                gameObject.GetComponent<FrogMiniboss>().Reset(savedPhaseNumber);
                break;
            case AttachedBoss.FROG_KING:
                gameObject.GetComponent<FrogKingScript>().Reset(savedPhaseNumber);
                break;
        }

        transform.position = savedPosition;
    }
}
