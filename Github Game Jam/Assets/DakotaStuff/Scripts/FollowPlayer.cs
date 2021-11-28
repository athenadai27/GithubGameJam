using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public int currentIndex;

    private float xMin, xMax, yMin, yMax;
    private float camY,camX;
    private float camOrthsize;
    private float cameraRatio;
    private Camera mainCam;
    public float smoothing = .5f;
    public Vector3 smoothPosition;
    public GameObject marker;
    public float followAhead;
    public GameObject player;
    public bool verticalFollow;
    public Vector3 scaleTracker;
    public bool cameraShaking;
    public List<BoxCollider2D> mapBounds;
    void Start()
    {
        
        xMin = mapBounds[currentIndex].bounds.min.x ;
        xMax = mapBounds[currentIndex].bounds.max.x;
        yMin = mapBounds[currentIndex].bounds.min.y;
        yMax = mapBounds[currentIndex].bounds.max.y;

        mainCam = GetComponent<Camera>();
        
        camOrthsize = mainCam.orthographicSize;
        cameraRatio = camOrthsize*mainCam.aspect;
        
    }
    void FixedUpdate(){
        float previousIndex = currentIndex;
        for(int i = 0; i < mapBounds.Count;i++){
            if(player.transform.position.x > mapBounds[i].bounds.min.x && player.transform.position.x < mapBounds[i].bounds.max.x && 
            player.transform.position.y > mapBounds[i].bounds.min.y && player.transform.position.y < mapBounds[i].bounds.max.y){
                currentIndex = i;
                
            }
        }
        if(previousIndex != currentIndex){
            ChangeBounds();
        }
        
        
        if(verticalFollow){
            camY = Mathf.Clamp(target.position.y + followAhead , yMin + camOrthsize, yMax - camOrthsize);
        } else{
            camY = Mathf.Clamp(target.position.y , yMin + camOrthsize, yMax - camOrthsize);
        }
        
        
        if(player.transform.lossyScale.x > 0){
            camX = Mathf.Clamp(target.position.x+ followAhead, xMin + cameraRatio, xMax - cameraRatio);
            smoothPosition = Vector3.Lerp(transform.position,new Vector3(camX ,camY,transform.position.z),smoothing*Time.deltaTime);
        } else{
            camX = Mathf.Clamp(target.position.x- followAhead, xMin + cameraRatio, xMax - cameraRatio);
            smoothPosition = Vector3.Lerp(transform.position,new Vector3(camX ,camY,transform.position.z),smoothing*Time.deltaTime);
        }
//        Debug.Log(smoothPosition);
        transform.position = smoothPosition;
    }
  

    public void ChangeBounds(){
         xMin = mapBounds[currentIndex].bounds.min.x;
        xMax = mapBounds[currentIndex].bounds.max.x;
        yMin = mapBounds[currentIndex].bounds.min.y;
        yMax = mapBounds[currentIndex].bounds.max.y;

        
        
        // camOrthsize = mainCam.orthographicSize;
        // cameraRatio = camOrthsize*mainCam.aspect;
        
    }

    public IEnumerator CameraShake(float duration, float magnitude){
        Vector3 originalPos = transform.position;
        cameraShaking = true;
        float elapsed = 0.0f;
        while(elapsed < duration){
            float x = Random.Range(-1f,1f)*magnitude;
            float y = Random.Range(-1f,1f)*magnitude;
            transform.position += new Vector3(x,y,0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraShaking = false;
        transform.position = originalPos;
    }

    public void FocusPlayer(BoxCollider2D newMapBounds){
        currentIndex = mapBounds.FindIndex(x => x == newMapBounds);
        ChangeBounds();
         if(verticalFollow){
            camY = Mathf.Clamp(target.position.y + followAhead , yMin + camOrthsize, yMax - camOrthsize);
        } else{
            camY = Mathf.Clamp(target.position.y , yMin + camOrthsize, yMax - camOrthsize);
        }
        
        
        if(player.transform.lossyScale.x > 0){
            camX = Mathf.Clamp(target.position.x+ followAhead, xMin + cameraRatio, xMax - cameraRatio);
        } else{
            camX = Mathf.Clamp(target.position.x- followAhead, xMin + cameraRatio, xMax - cameraRatio);
        }
        transform.position = new Vector3(camX ,camY,transform.position.z);
    }
}
