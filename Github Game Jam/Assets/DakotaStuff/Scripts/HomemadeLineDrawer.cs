using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomemadeLineDrawer : MonoBehaviour
{
    public float minDistanceForNextPoint;
    public Vector3[] vertices;
    public int[] triangles;
    Mesh mesh;

    public enum state { drawing, notDrawing };
    public state drawState = state.notDrawing;
    public List<Vector3> points;
    public float lineWidth;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


    }

    // Update is called once per frame
    void Update()
    {
        switch (drawState)
        {
            case state.drawing:
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                if (Vector3.Distance(mousePos, points[points.Count - 1]) > minDistanceForNextPoint)
                {
                    points.Add(mousePos);
                }
                if (points.Count > 1)
                {
                    CreateShape();
                    UpdateMesh();
                }

                break;
            case state.notDrawing:
                if (Input.GetKeyDown(KeyCode.D))
                {
                    drawState = state.drawing;
                    Vector3 mousePosInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosInit.z = 0;
                    points.Add(mousePosInit);
                }
                break;

        }
    }

    void CreateShape()
    {
        List<Vector3> verticesList = new List<Vector3>();
        for (int i = 0; i < points.Count; i++)
        {
            if (i < points.Count - 1)
            {
                Vector3 vectorDir = points[i + 1] - points[i];
                vectorDir = Quaternion.AngleAxis(90, Vector3.forward) * vectorDir;
                vectorDir.Normalize();
                vectorDir *= lineWidth;
                verticesList.Add(points[i] + vectorDir * .5f);
                verticesList.Add(points[i] - vectorDir * .5f);
            }
            else
            {
                Vector3 vectorDir = points[i] - points[i - 1];
                vectorDir = Quaternion.AngleAxis(90, Vector3.forward) * vectorDir;
                vectorDir.Normalize();
                vectorDir *= lineWidth;
                verticesList.Add(points[i] + vectorDir * .5f);
                verticesList.Add(points[i] - vectorDir * .5f);
            }
        }
        vertices = verticesList.ToArray();

        List<int> triangleList = new List<int>();
        for (int i = 0; i < verticesList.Count - 1; i += 2)
        {
            
            if (i < verticesList.Count  - 2)
            {
                triangleList.Add(i);
                triangleList.Add(i + 1);
                triangleList.Add(i + 2);
            }

        }
        for (int i = 1; i < verticesList.Count; i += 2)
        {
            
            if (i < verticesList.Count  - 2)
            {
                triangleList.Add(i);
                triangleList.Add(i + 2);
                triangleList.Add(i + 1);
            }
        }
        triangles = triangleList.ToArray();
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
