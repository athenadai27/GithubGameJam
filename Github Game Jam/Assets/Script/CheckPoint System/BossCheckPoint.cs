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

    /// <summary>
    /// save progress for Garbara.
    /// </summary>
    /// <param name="phase">convert the current boss state to an integer</param>
    public void SaveForGabara(int phase)
    {
        savedPhaseNumber = phase;
        attachedBoss = AttachedBoss.GARBARA;
    }

    /// <summary>
    /// save progress for Frog King
    /// </summary>
    /// <param name="phase">convert the current boss state to an integer</param>
    public void SaveForFrogKing(int phase)
    {
        savedPhaseNumber = phase;
        attachedBoss = AttachedBoss.FROG_KING;
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
    }
}
