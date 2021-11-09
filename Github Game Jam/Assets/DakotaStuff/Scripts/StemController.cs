using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private List<Vector3> linePoints = new List<Vector3>();

    [SerializeField]
    private float minDistanceForNextPoint;
    private Camera mainCam;

    [SerializeField]
    private GameObject flower;

    [SerializeField]
    private LayerMask collisionMask;

    [SerializeField]
    private Vector3 previousMousePos;

    public bool lineActive;

    public enum PlayerStates { drawing, nothing, retracting };
    public PlayerStates playerState;

    private Vector3 startPos;
    private Vector3 endPos;
    private float lerpToPoint;
    [SerializeField]
    private float retractSpeed;
    [SerializeField]
    private Transform flowerHolder;

    public GrabbableWord grabbedWord;

    [SerializeField]
    private float sensitivity;

    private EdgeCollider2D edgeCollider2D;

    [SerializeField]
    private float lineWidth;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = false;
        playerState = PlayerStates.nothing;
        edgeCollider2D = lineRenderer.GetComponent<EdgeCollider2D>();
        edgeCollider2D.points = new Vector2[lineRenderer.positionCount];
        for(int i = 0; i < lineRenderer.positionCount;i++){
            edgeCollider2D.points[i] = (Vector2)lineRenderer.GetPosition(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        linePoints[0] = lineRenderer.transform.position;
        lineRenderer.SetPosition(1, flowerHolder.transform.position);
        linePoints[1] = flowerHolder.transform.position;

        switch (playerState)
        {
            case PlayerStates.drawing:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    playerState = PlayerStates.retracting;
                    lerpToPoint = 0;
                    startPos = linePoints[linePoints.Count - 1];

                    if (linePoints.Count > 1)
                    {
                        endPos = linePoints[linePoints.Count - 2];
                    }
                    else
                    {
                        endPos = Vector3.zero;
                    }
                    return;
                }
                //linePoints[0] = transform.position;
                // lineRenderer.SetPosition(0, transform.position);

                Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                // Vector3 mouseDir = mousePos - previousMousePos;
                Vector3 mouseDir = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * sensitivity;

                if (mouseDir != Vector3.zero)
                {
                    Vector3 secondFromEndPoint = linePoints[linePoints.Count - 2];
                 //   Debug.DrawRay(linePoints[linePoints.Count - 1] + mouseDir * .1f, mouseDir * 100f, Color.red);
                    RaycastHit2D leftRaycast = Physics2D.Raycast((Vector2)linePoints[linePoints.Count - 1] + (Vector2)mouseDir*.01f + Vector2.Perpendicular(mouseDir).normalized*lineWidth, mouseDir, .5f, collisionMask);
                    RaycastHit2D rightRaycast = Physics2D.Raycast((Vector2)linePoints[linePoints.Count - 1] + (Vector2)mouseDir*.01f - Vector2.Perpendicular(mouseDir).normalized*lineWidth, mouseDir, .5f, collisionMask);
                    RaycastHit2D midRaycast = Physics2D.Raycast(linePoints[linePoints.Count - 1] + mouseDir*.01f, mouseDir, .5f, collisionMask);
                    if (!leftRaycast && !rightRaycast && !midRaycast)
                    {
                        flower.transform.position = linePoints[linePoints.Count - 1] + mouseDir;

                        linePoints[linePoints.Count - 1] = linePoints[linePoints.Count - 1] + mouseDir;
                        // if (Vector3.Distance(linePoints[linePoints.Count - 1], secondFromEndPoint) <= minDistanceForNextPoint)
                        // {
                        //     linePoints.RemoveAt(linePoints.Count - 1);
                        //     Debug.Log("remove");
                        // }
                        // else 
                        if (Vector3.Distance(linePoints[linePoints.Count - 1], secondFromEndPoint) > minDistanceForNextPoint)
                        {
                            linePoints.Add(linePoints[linePoints.Count - 1]);


                        }
                        lineRenderer.positionCount = linePoints.Count;
                        lineRenderer.SetPositions(linePoints.ToArray());
                        Vector2[] edgePoints = new Vector2[linePoints.Count];
                        for (int i = 0; i < linePoints.Count; i++)
                        {

                            edgePoints[i].x = linePoints[i].x - lineRenderer.transform.position.x;
                            edgePoints[i].y = linePoints[i].y - lineRenderer.transform.position.y;


                        }
                        edgeCollider2D.points = edgePoints;

                    }
                    else
                    {
                        //Debug.Log(Physics2D.Raycast(linePoints[linePoints.Count - 1] + mouseDir*.001f, mouseDir, .5f, collisionMask).collider.gameObject.name);
                    }
                }



                previousMousePos = mousePos;

                break;
            case PlayerStates.retracting:


                if (flower.transform.position != flowerHolder.position)
                {
                    lerpToPoint += Time.deltaTime * retractSpeed;
                    flower.transform.position = Vector3.Lerp(startPos, endPos, lerpToPoint);
                    linePoints[linePoints.Count - 1] = flower.transform.position;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, flower.transform.position);
                    if (flower.transform.position == endPos)
                    {
                        lerpToPoint = 0;


                        if (linePoints.Count > 2)
                        {
                            linePoints.RemoveAt(linePoints.Count - 1);
                            lineRenderer.positionCount--;
                            startPos = linePoints[linePoints.Count - 1];
                            endPos = linePoints[linePoints.Count - 2];
                             Vector2[] edgePoints = new Vector2[linePoints.Count];
                        for (int i = 0; i < linePoints.Count; i++)
                        {

                            edgePoints[i].x = linePoints[i].x - lineRenderer.transform.position.x;
                            edgePoints[i].y = linePoints[i].y - lineRenderer.transform.position.y;


                        }
                        edgeCollider2D.points = edgePoints;
                        }
                        else
                        {
                            flower.transform.position = flowerHolder.position;
                            //endPos = flowerHolder.position;
                        }
                    }
                }
                else
                {
                    if (grabbedWord != null)
                    {
                        grabbedWord.SpawnItem();
                        grabbedWord = null;
                    }

                    playerState = PlayerStates.nothing;
                }
                break;

            case PlayerStates.nothing:

                if (Input.GetKeyDown(KeyCode.L))
                {
                    Vector3 mousePosInit = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    mousePosInit.z = 0;
                    previousMousePos = mousePosInit;
                    playerState = PlayerStates.drawing;
                    linePoints[0] = transform.position;
                    linePoints[1] = flowerHolder.position;

                    lineRenderer.SetPositions(linePoints.ToArray());
                }
                break;
        }
    }
}
