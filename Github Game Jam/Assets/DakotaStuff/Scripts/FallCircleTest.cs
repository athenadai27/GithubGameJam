using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCircleTest : MonoBehaviour
{
    public Vector3 endScale;
    public Vector3 startScale;
    public float scaleLerp;
    public float alphaLerp;
    public SpriteRenderer sprite;
    public bool grow;
    public Transform garbaraTransform;
    public float garbaraFloorY;
    public float garbaraCeiling;
    // Start is called before the first frame update
    void OnEnable()
    {
        scaleLerp = 0;
        alphaLerp = 0;
        //garbaraFloorY = garbaraTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = garbaraTransform.position.x;
        transform.position = newPosition;
        float garbaraY = garbaraTransform.position.y - garbaraFloorY;
        Debug.Log(garbaraY);
        float amountThrough = garbaraY / (garbaraCeiling);
        Color spriteColor = sprite.color;
        spriteColor.a = 1 - amountThrough;
        sprite.color = spriteColor;

        // at garbarafloor+20, x == 2.4;
        // if garbarafloor, x == 1.8;
        transform.localScale = new Vector3(endScale.x + (startScale.x - endScale.x) * amountThrough, .6f, 1f);
        // if (grow)
        // {
        //     scaleLerp += Time.deltaTime;
        //     alphaLerp += Time.deltaTime;
        //     Color spriteColor = sprite.color;
        //     // if garbara.y == floory + 20, alpha = 0;
        //     // if garbara.y == floory, alpha = 1;
        //     float amountThrough = garbaraY/(garbaraFloorY+20f);
        //     spriteColor.a = 1 - amountThrough;
        //     sprite.color = spriteColor;

        //     // at garbarafloor+20, x == 2.4;
        //     // if garbarafloor, x == 1.8;
        //     transform.localScale = new Vector3(startScale.x + (endScale.x-startScale.x)*amountThrough,.6f,1f);
        //     //spriteColor.a = Mathf.Lerp(0, 1, alphaLerp);

        //     transform.localScale = Vector3.Lerp(startScale, endScale, scaleLerp);
        // }
        // else
        // {
        //     scaleLerp += Time.deltaTime;
        //     alphaLerp += Time.deltaTime;
        //     Color spriteColor = sprite.color;
        //     spriteColor.a = Mathf.Lerp(1, 0, alphaLerp);
        //     sprite.color = spriteColor;
        //     transform.localScale = Vector3.Lerp(endScale, startScale, scaleLerp);
        // }

    }
    public void SetTop(float newTop){
        garbaraCeiling = newTop-garbaraFloorY;
    }
}
