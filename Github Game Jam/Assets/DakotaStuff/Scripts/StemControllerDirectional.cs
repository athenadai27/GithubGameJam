using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemControllerDirectional : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector3> points;
    public int previousDirection;
    public GameObject flowerHolder;
    public GameObject flower;
    public enum direction { up, down, left, right };
    public direction stemDirection = direction.up;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, flowerHolder.transform.position);
        points[0] = transform.position;
        points[1] = flowerHolder.transform.position;
        if (Input.GetKeyDown(KeyCode.W) && stemDirection != direction.up)
        {
            stemDirection = direction.up;
             points.Add(points[points.Count-1]);
             lineRenderer.positionCount = points.Count;
             lineRenderer.SetPosition(lineRenderer.positionCount-1,points[points.Count-1]);
        } else if(Input.GetKeyDown(KeyCode.A)){
            stemDirection = direction.left;
            points.Add(points[points.Count-1]);
             lineRenderer.positionCount = points.Count;
             lineRenderer.SetPosition(lineRenderer.positionCount-1,points[points.Count-1]);
        } else if(Input.GetKeyDown(KeyCode.D)){
            stemDirection = direction.right;
            points.Add(points[points.Count-1]);
             lineRenderer.positionCount = points.Count;
             lineRenderer.SetPosition(lineRenderer.positionCount-1,points[points.Count-1]);
        } else if(Input.GetKeyDown(KeyCode.S)){
            stemDirection = direction.down;
            points.Add(points[points.Count-1]);
             lineRenderer.positionCount = points.Count;
             lineRenderer.SetPosition(lineRenderer.positionCount-1,points[points.Count-1]);
        }
        flower.transform.position = points[points.Count-1];
        switch (stemDirection)
        {
            case direction.up:
                if (Input.GetKey(KeyCode.W))
                {
                    points[points.Count - 1] += Vector3.up * Time.deltaTime;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, points[points.Count - 1]);
                }
                break;
            case direction.left:
                if (Input.GetKey(KeyCode.A))
                {
                    points[points.Count - 1] += Vector3.left * Time.deltaTime;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, points[points.Count - 1]);
                }
                break;
            case direction.right:
                if (Input.GetKey(KeyCode.D))
                {
                    points[points.Count - 1] += Vector3.right * Time.deltaTime;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, points[points.Count - 1]);
                }
                break;
            case direction.down:
                if (Input.GetKey(KeyCode.S))
                {
                    points[points.Count - 1] += Vector3.down * Time.deltaTime;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, points[points.Count - 1]);
                }
                break;
        }
    }
}
