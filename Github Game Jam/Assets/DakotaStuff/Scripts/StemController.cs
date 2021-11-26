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
    public GameObject grabbedItem;

    [SerializeField]
    private float sensitivity;

    private EdgeCollider2D edgeCollider2D;

    [SerializeField]
    private float lineWidth;

    [SerializeField]
    private float stemLimit;
    [SerializeField]
    private float currentStemLength;
    [SerializeField]
    private LayerMask wordMask;
    [SerializeField]
    private CircleCollider2D flowerCollider;

    [SerializeField]
    Transform handTransform;

    public PlayerController playerController;
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = false;
        playerState = PlayerStates.nothing;
        edgeCollider2D = lineRenderer.GetComponent<EdgeCollider2D>();
        edgeCollider2D.points = new Vector2[lineRenderer.positionCount];
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            edgeCollider2D.points[i] = (Vector2)lineRenderer.GetPosition(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = playerTransform.localScale;
        lineRenderer.SetPosition(0, Vector3.zero);
        linePoints[0] = Vector3.zero;
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(flowerHolder.transform.position));
        linePoints[1] = transform.InverseTransformPoint(flowerHolder.transform.position);
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (grabbedItem != null)
            {
                grabbedItem.GetComponent<ItemScript>().Drop();
                grabbedItem = null;
            }
        }
        switch (playerState)
        {
            case PlayerStates.drawing:
                if (Input.GetMouseButtonDown(0))
                {
                    Retract();

                    return;
                }else if(Input.GetMouseButton(0)){
                     Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Vector3 mouseDir = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0) * sensitivity;

                if (mouseDir != Vector3.zero && currentStemLength < stemLimit)
                {
                    Vector3 secondFromEndPoint = linePoints[linePoints.Count - 2];
                    RaycastHit2D leftRaycast = Physics2D.Raycast(transform.TransformPoint((Vector2)linePoints[linePoints.Count - 1] + (Vector2)mouseDir * .01f + Vector2.Perpendicular(mouseDir).normalized * lineWidth), mouseDir, .1f, collisionMask);
                    RaycastHit2D rightRaycast = Physics2D.Raycast(transform.TransformPoint((Vector2)linePoints[linePoints.Count - 1] + (Vector2)mouseDir * .01f - Vector2.Perpendicular(mouseDir).normalized * lineWidth), mouseDir, .1f, collisionMask);
                    RaycastHit2D midRaycast = Physics2D.Raycast(transform.TransformPoint(linePoints[linePoints.Count - 1] + mouseDir * .01f), mouseDir, .1f, collisionMask);
                    if (!leftRaycast && !rightRaycast && !midRaycast)
                    {
                        flower.transform.position = transform.TransformPoint(linePoints[linePoints.Count - 1] + mouseDir);

                        linePoints[linePoints.Count - 1] = linePoints[linePoints.Count - 1] + mouseDir;

                        if (Vector3.Distance(linePoints[linePoints.Count - 1], secondFromEndPoint) > minDistanceForNextPoint)
                        {
                            currentStemLength += Vector3.Distance(linePoints[linePoints.Count - 1], linePoints[linePoints.Count - 2]);
                            linePoints.Add(linePoints[linePoints.Count - 1]);


                        }
                        lineRenderer.positionCount = linePoints.Count;
                        lineRenderer.SetPositions(linePoints.ToArray());
                        Vector2[] edgePoints = new Vector2[linePoints.Count];

                        for (int i = 0; i < linePoints.Count; i++)
                        {

                            edgePoints[i].x = linePoints[i].x;
                            edgePoints[i].y = linePoints[i].y;


                        }
                        edgeCollider2D.points = edgePoints;

                    }
                    else
                    {
                        //Debug.Log(Physics2D.Raycast(linePoints[linePoints.Count - 1] + mouseDir*.001f, mouseDir, .5f, collisionMask).collider.gameObject.name);
                    }
                }



                previousMousePos = mousePos;

                }

               
                break;
            case PlayerStates.retracting:


                if (flower.transform.position != flowerHolder.position)
                {
                    
                    lerpToPoint += Time.deltaTime * retractSpeed;
                    flower.transform.position = transform.TransformPoint(Vector3.Lerp(startPos, endPos, lerpToPoint));
                    linePoints[linePoints.Count - 1] = transform.InverseTransformPoint(flower.transform.position);
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.InverseTransformPoint(flower.transform.position));
                    if (Vector3.Distance(transform.InverseTransformPoint(flower.transform.position), endPos) < .1f)
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

                                edgePoints[i].x = linePoints[i].x;
                                edgePoints[i].y = linePoints[i].y;


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
                        
                        grabbedItem = grabbedWord.SpawnItem();
                        grabbedItem.SetActive(true);
                        
                        grabbedWord.Reset();
                        grabbedWord = null;
                    }
                    playerController.Uproot();
                    currentStemLength = 0;
                    playerState = PlayerStates.nothing;
                }
                break;

            case PlayerStates.nothing:

                if (Input.GetMouseButtonDown(0) && playerController.grounded)
                {
                    Debug.Log("go");
                    Vector3 mousePosInit = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    mousePosInit.z = 0;
                    previousMousePos = mousePosInit;
                    playerState = PlayerStates.drawing;
                    linePoints[0] = Vector3.zero; ;
                    linePoints[1] = transform.InverseTransformPoint(flowerHolder.position);

                    lineRenderer.SetPositions(linePoints.ToArray());

                    playerController.Plant();
                    // playerScale = playerController.transform.localScale;
                }
                break;
        }
    }

    public void Retract()
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
    }

    public void Reset(){
        playerState = PlayerStates.nothing;
        if(grabbedItem != null){
            Destroy(grabbedItem);
            grabbedItem = null;
        }
        if(grabbedWord != null){
            grabbedWord.Reset();
        }
        linePoints.Clear();
        linePoints.Add(Vector3.zero);
        linePoints.Add(transform.InverseTransformPoint(flowerHolder.transform.position));
        lineRenderer.positionCount = 2;
       lineRenderer.SetPosition(0, Vector3.zero);
 
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(flowerHolder.transform.position));
        flower.transform.position = flowerHolder.transform.position;

    }
}
