using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemPathfinding : MonoBehaviour
{
    public List<Vector3> positions;
    public List<Vector3> path;
    public Vector3 targetPos;
    public float moveSpeed;
    public List<float> costsToPoint;
    public List<float> costsToPlayer;
    public LayerMask obstacles;
    public List<Vector3> parents;
    public List<Vector3> bestPath;
    public List<Vector3> closedList;
    public List<Vector3> closedListParents;
    public float radius;
    public float tileSpace;
    public bool roaming;
    public float updatePathTime;
    public float waitTime;
    public bool waiting;
    public LineRenderer lineRenderer;

    public enum PlayerStates { drawing, notDrawing };
    public PlayerStates playerState;

    public Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        playerState = PlayerStates.notDrawing;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > updatePathTime)
        {
            updatePathTime = Time.time + .1f;
            UpdatePath();
        }
        if (path.Count > 0)
        {
            

        }
        else
        {
           

        }
    }

    public void UpdatePath()
    {
        path.Clear();
        positions.Clear();
        costsToPoint.Clear();
        costsToPlayer.Clear();
        closedList.Clear();
        parents.Clear();
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        targetPos = new Vector3(mousePos.x, mousePos.y, 0);
        Vector3 initialPos = new Vector3(transform.position.x, transform.position.y, 0); 
        closedList.Add(initialPos);
        costsToPoint.Add(0);
        Vector3 vectorDif = targetPos - initialPos;
        float pathValue;
        if (Mathf.Abs(vectorDif.x) > Mathf.Abs(vectorDif.y))
        {
            float xyDif = Mathf.Abs(vectorDif.x) - Mathf.Abs(vectorDif.y);
            pathValue = xyDif + Mathf.Abs(vectorDif.y) * 1.4f;
        }
        else if (Mathf.Abs(vectorDif.x) < Mathf.Abs(vectorDif.y))
        {
            float xyDif = Mathf.Abs(vectorDif.y) - Mathf.Abs(vectorDif.x);
            pathValue = xyDif + Mathf.Abs(vectorDif.x) * 1.4f;
        }
        else
        {
            pathValue = Mathf.Abs(vectorDif.x) * 1.4f;
        }

        costsToPlayer.Add(pathValue);
        Vector3 currentPathPos = initialPos;
        positions.Add(currentPathPos);
        parents.Add(Vector3.one * 500);

        int breakvalue = 0;
        while (currentPathPos != targetPos && breakvalue < 1000)
        {
            int currentPathIndex = positions.FindIndex(x => x == currentPathPos);
            currentPathPos = ReturnPoint(currentPathPos, currentPathIndex);
            closedList.Add(currentPathPos);
            if(Vector3.Distance(currentPathPos,targetPos) < tileSpace*1.4f){
                positions[positions.FindIndex(x => x == currentPathPos)] = targetPos;
                currentPathPos = targetPos;
                
                
            }
            breakvalue++;
        }
        path.Add(currentPathPos);
        int nextBreakValue = 0;
        while (currentPathPos != initialPos && nextBreakValue < 100)
        {
            nextBreakValue++;
            int parentIndex = positions.FindIndex(x => x == currentPathPos);
            if (parentIndex != -1)
            {
                currentPathPos = parents[parentIndex];
            }
            else
            {
                break;
            }


            path.Add(currentPathPos);
        }

        path.Reverse();
        lineRenderer.positionCount = path.Count;
        for(int i = 0; i < lineRenderer.positionCount;i++){
            lineRenderer.SetPosition(i,path[i]);
        }
        

    }
    //1. If tile is not current tile
    public Vector3 ReturnPoint(Vector3 currentPos, int currentIndex)
    {
        for (float x = -1 ; x <= 1 ; x++)
        {
            for (float y = -1 ; y <= 1 ; y++)
            {
                if (!(x == 0 && y == 0))
                {
                    Vector3 newPosition = currentPos + new Vector3(x*tileSpace, y*tileSpace, 0);

                    Vector3 vectorDif = targetPos - newPosition;
                    float pathValue;
                    if (Mathf.Abs(vectorDif.x) > Mathf.Abs(vectorDif.y))
                    {
                        float xyDif = Mathf.Abs(vectorDif.x) - Mathf.Abs(vectorDif.y);
                        pathValue = xyDif + Mathf.Abs(vectorDif.y) * 1.4f;
                    }
                    else if (Mathf.Abs(vectorDif.x) < Mathf.Abs(vectorDif.y))
                    {
                        float xyDif = Mathf.Abs(vectorDif.y) - Mathf.Abs(vectorDif.x);
                        pathValue = xyDif + Mathf.Abs(vectorDif.x) * 1.4f;
                    }
                    else
                    {
                        pathValue = Mathf.Abs(vectorDif.x) * 1.4f;
                    }

                    if (Physics2D.OverlapCircle((Vector2)newPosition, radius, obstacles) == null)
                    {

                        if (x == 0 || y == 0)
                        {
                            float cost = costsToPoint[currentIndex] + 1*tileSpace;

                            if (positions.Contains(newPosition))
                            {
                                int positionIndex = positions.FindIndex(z => z == newPosition);
                                if (costsToPoint[positionIndex] > cost)
                                {
                                    costsToPoint[positionIndex] = cost;
                                    costsToPlayer[positionIndex] = pathValue;
                                    parents[positionIndex] = currentPos;
                                }
                            }
                            else
                            {
                                positions.Add(newPosition);
                                costsToPoint.Add(cost);
                                costsToPlayer.Add(pathValue);
                                parents.Add(currentPos);
                            }
                        }
                        else
                        {

                            float cost = costsToPoint[currentIndex] + 1.4f*tileSpace;
                            if (positions.Contains(newPosition))
                            {
                                int positionIndex = positions.FindIndex(z => z == newPosition);
                                if (costsToPoint[positionIndex] > cost)
                                {
                                    costsToPoint[positionIndex] = cost;
                                    costsToPlayer[positionIndex] = pathValue;
                                    parents[positionIndex] = currentPos;
                                }

                            }
                            else
                            {

                                positions.Add(newPosition);
                                costsToPoint.Add(cost);
                                costsToPlayer.Add(pathValue);
                                parents.Add(currentPos);
                            }
                        }
                    }

                }
            }
        }
        int bestIndex = 0;
        float maxValue = Mathf.Infinity;
        for (int i = 1; i < positions.Count; i++)
        {
            if (costsToPoint[i] + costsToPlayer[i] < maxValue && !closedList.Contains(positions[i]))
            {
                bestIndex = i;
                maxValue = costsToPoint[i] + costsToPlayer[i];
            }
        }
     
        return positions[bestIndex];

    }
}
