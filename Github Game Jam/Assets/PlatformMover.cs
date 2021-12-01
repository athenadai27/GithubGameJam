using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public List<Transform> platformTransforms;
    public enum MoveStates {waiting, moving};
    public MoveStates moveState;
    public List<Transform> platformStartTransforms;
    public List<Transform> platformMoveTransforms;
    public List<Vector3> startPositions;
    public List<Vector3> endPositions;
    public float moveLerp;
    public float arriveInSeconds;
    public float waitTime;
    public float waitTimeAdditional;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < platformStartTransforms.Count;i++){
            startPositions[i] = platformStartTransforms[i].position;
        }
        for(int i = 0; i < platformMoveTransforms.Count;i++){
            endPositions[i] = platformMoveTransforms[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(moveState){
            case MoveStates.waiting:
                if(Time.time > waitTime){
                    moveState = MoveStates.moving;
                }
                break;
            case MoveStates.moving:
                moveLerp += Time.deltaTime/arriveInSeconds;
                for(int i = 0; i < platformStartTransforms.Count;i++){
                    platformStartTransforms[i].position = Vector3.Lerp(startPositions[i],endPositions[i],moveLerp);
                }
                if(moveLerp >= 1){
                    List<Vector3> startPositionsPlaceholder = startPositions;
                    startPositions = endPositions;
                    endPositions = startPositionsPlaceholder;
                    moveLerp = 0f;
                    moveState = MoveStates.waiting;
                    waitTime = Time.time + waitTimeAdditional;
                }
                break;
        }
    }
}
