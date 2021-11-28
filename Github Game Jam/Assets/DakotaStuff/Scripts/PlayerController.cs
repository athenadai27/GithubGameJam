using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform groundTransform;

    public BoxCollider2D groundCollider;
    public CapsuleCollider2D bodyCollider;
    public bool grounded;
    public bool isJumping;
    public bool startJump;
    public LayerMask groundMask;
    public LayerMask collisionMask;
    public LayerMask hideMask;
    public Vector2 moveVector;
    public float moveSpeed;
    public float jumpHeight;
    public float customGravity;
    public float airTimeMax;
    public float airTime;
    public float maxGravity;
    public float moveDir;
    public float lerpTime;
    public float previousHealth;
    public Animator myAnim;
    public enum PlayerStates { normal, hiding, restricted, planted, disabled};
    public bool canMove;
    public PlayerStates playerState = PlayerStates.normal;
    public SpriteRenderer sprite;

    public GameObject emptyObject;
    public int jumpTracker;
    public int maxJumpTrackNum;
    public float groundedLeewayTime;
    public bool previousGrounded;
    public bool newGrounded;
    public bool haveJumped;
    public bool canDoubleJump;
    public bool doubleJumpOnCooldown;
    public string groundTag;
    public Vector3 rightDirection;

    public GameObject hidingObject;
    
    public Transform flowerHolderTransform;
    public LineRenderer lineRenderer;
    public SpriteRenderer flowerSprite;

    public StemController stemController;
    public Checkpoint checkpoint;
    // Start is called before the first frame update
    void Start()
    {
        //  Application.targetFrameRate = 30;
        Cursor.lockState =  CursorLockMode.Confined;
        myAnim = GetComponent<Animator>();
        if (PlayerPrefs.GetInt("HealthStars") == 0)
        {
            PlayerPrefs.SetInt("HealthStars", 4);
        }
    }

    // Update is called once per frame
    void Update()
    {

        moveVector = Vector3.zero;

        //if(grounded){
        if(canMove){
            moveDir = Input.GetAxisRaw("Horizontal");
        } else{
            moveDir = 0;
        }
        


        if (transform.parent != null)
        {
            //   transform.Rotate(0, 0, 0);
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
        switch (playerState)
        {
            case PlayerStates.normal:

                if (Input.GetKeyDown(KeyCode.C))
                {
                    Collider2D hideCheck = Physics2D.OverlapCapsule(bodyCollider.bounds.center, bodyCollider.bounds.size, bodyCollider.direction, 0, hideMask);
                    if (hideCheck)
                    {
                        
                        Hide(hideCheck.gameObject);
                        return;
                    }
                    else
                    {
                        if (myAnim.GetBool("Crouching"))
                        {
                            moveSpeed = 8f;
                            myAnim.SetBool("Crouching", false);
                            flowerHolderTransform.localPosition = new Vector3(.02f,2.48f,0f);
                            lineRenderer.transform.localPosition = new Vector3(.02f,1.83f,0f);
                        }
                        else
                        {
                            moveSpeed = 5f;
                            myAnim.SetBool("Crouching", true);
                            flowerHolderTransform.localPosition = new Vector3(.02f,2.15f,0f);
                            lineRenderer.transform.localPosition = new Vector3(.02f,1.5f,0f);
                        }
                    }


                }

                Vector3 currentScale = transform.localScale;
                Vector3 currentBoundsPos = groundCollider.bounds.center;
                if (moveDir > 0)
                {
                    if (transform.parent != null)
                    {
                        transform.localScale = new Vector3(1 / transform.parent.lossyScale.x, 1 / transform.parent.lossyScale.y, 1);

                        if (currentScale.x != transform.localScale.x)
                        {
                            Physics2D.SyncTransforms();
                            transform.position += Vector3.left * (groundCollider.bounds.center.x - currentBoundsPos.x);
                        }

                    }
                    else
                    {
                        transform.localScale = Vector3.one;
                        if (currentScale.x != transform.localScale.x)
                        {
                            Physics2D.SyncTransforms();
                            transform.position += Vector3.left * (groundCollider.bounds.center.x - currentBoundsPos.x);
                        }

                    }

                }
                else if (moveDir < 0)
                {
                    if (transform.parent != null)
                    {
                        transform.localScale = new Vector3(-1 / transform.parent.lossyScale.x, 1 / transform.parent.lossyScale.y, 1);
                        if (currentScale.x != transform.localScale.x)
                        {
                            Physics2D.SyncTransforms();
                            transform.position += Vector3.left * (groundCollider.bounds.center.x - currentBoundsPos.x);
                        }

                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        if (currentScale.x != transform.localScale.x)
                        {
                            Physics2D.SyncTransforms();
                            transform.position += Vector3.left * (groundCollider.bounds.center.x - currentBoundsPos.x);
                        }

                    }
                }
                // if(transform.localScale != stemController.playerScale){
                //     stemController.transform.localScale = new Vector3(-1f,1f,1f);
                // } else{
                //     stemController.transform.localScale = Vector3.one;
                // }

                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && canMove)
                {
                    if (grounded)
                    {
                        jumpTracker = 0;
                        isJumping = true;
                        haveJumped = true;
                        airTime = Time.time + airTimeMax;
                        startJump = true;
                    }
                    else
                    {
                        if (Time.time < groundedLeewayTime && !haveJumped)
                        {
                            jumpTracker = 0;
                            isJumping = true;
                            haveJumped = true;
                            airTime = Time.time + airTimeMax;
                            startJump = true;

                        }

                    }

                }
                if (Input.GetKeyDown(KeyCode.S) && grounded)
                {
                    RaycastHit2D groundedRay = Physics2D.BoxCast(groundCollider.bounds.center, groundCollider.bounds.size - Vector3.one * .01f, 0f, Vector2.down, .01f, groundMask);
                    if (groundedRay)
                    {
                        // if (groundedRay.collider.gameObject.GetComponent<PassThroughScript>() != null)
                        // {
                        //     PassThroughScript passThroughPlatform = groundedRay.collider.gameObject.GetComponent<PassThroughScript>();
                        //     passThroughPlatform.passThroughCollider.enabled = false;
                        //     passThroughPlatform.disableTime = Time.time + .5f;
                        //     passThroughPlatform.disabled = true;
                        // }
                    }
                }
                moveVector += new Vector2(moveDir * moveSpeed, 0);
                if (isJumping)
                {
                    if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)))
                    {
                        //airTime -= Time.deltaTime;
                        if (jumpTracker >= maxJumpTrackNum)
                        {
                            //        Debug.Log(transform.position.y);
                            isJumping = false;
                            customGravity = 0f;

                            //      Debug.Log("no longer jumping");
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
                    {
                        isJumping = false;
                        customGravity = 0f;

                    }
                    else
                    {
                        isJumping = false;
                        customGravity = 0f;

                    }
                }




                break;
            case PlayerStates.restricted:
                break;
            case PlayerStates.hiding:
            if (Input.GetKeyDown(KeyCode.C))
                {
                    Appear();
                }
                break;
            case PlayerStates.planted:
                break;
            case PlayerStates.disabled:
                break;
        }
        // }
    }

    void FixedUpdate()
    {
        if(playerState != PlayerStates.hiding){
            ColliderCheck();
        }
        

        switch (playerState)
        {

            case PlayerStates.normal:

                RaycastHit2D[] groundedRay = Physics2D.BoxCastAll(groundCollider.bounds.center, groundCollider.bounds.size - Vector3.one * .01f, 0f, Vector2.down, .01f, groundMask);
                RaycastHit2D hitGroundRay = new RaycastHit2D();
                for (int i = 0; i < groundedRay.Length; i++)
                {

                    if (Mathf.Abs(groundedRay[i].normal.x) < .5f && groundedRay[i].normal.y > 0)
                    {
                        hitGroundRay = groundedRay[i];
                    }
                }
                if (hitGroundRay.collider != null && !isJumping && hitGroundRay.point.y <= bodyCollider.bounds.center.y)
                {
                    Vector2 upHitPoint = hitGroundRay.point + Vector2.up * groundCollider.bounds.extents.y;
                    RaycastHit2D extraCheckRay = Physics2D.Raycast(upHitPoint, hitGroundRay.point - upHitPoint, groundCollider.bounds.extents.y - .01f, groundMask);

                    grounded = true;

                    haveJumped = false;
                    doubleJumpOnCooldown = false;
                    transform.SetParent(hitGroundRay.transform);



                }
                else
                {
                    if (grounded)
                    {
                        previousGrounded = true;
                        groundedLeewayTime = Time.time + .3f;
                    }
                    else
                    {
                        previousGrounded = false;
                    }
                    grounded = false;
                    transform.SetParent(null);
                }
                myAnim.SetBool("Grounded", grounded);
                if (grounded )
                {
                    rightDirection = Quaternion.Euler(0, 0, -transform.lossyScale.x * 90) * hitGroundRay.normal;

                    if (startJump)
                    {
                        transform.position += Vector3.up * jumpHeight * Time.fixedDeltaTime;
                        startJump = false;
                    }
                    else
                    {
                        Vector3 updatedMoveDir = Quaternion.Euler(0, 0, -moveDir * 90) * hitGroundRay.normal;

                        HorizontalMove(updatedMoveDir, moveSpeed);
                    }

                }
                else
                {
                    startJump = false;
                    rightDirection = Vector3.right * transform.lossyScale.x;
                    if (moveDir != 0 )
                    {

                        Vector3 horizontalMoveDir = Vector3.right * moveDir;
                        HorizontalMove(horizontalMoveDir, moveSpeed);
                    }
                    if (isJumping)
                    {
                        if (jumpTracker < maxJumpTrackNum)
                        {
                            JumpCheck();
                        }
                    }
                    else
                    {
                        VerticalMove();
                    }
                }
                break;
            case PlayerStates.restricted:
                break;
            case PlayerStates.hiding:
                break;
            case PlayerStates.planted:
                break;
            case PlayerStates.disabled:
                break;
        }


    }

    public void HorizontalMove(Vector3 updatedMoveDir, float updatedMoveSpeed)
    {
        if (moveDir != 0)
        {
            Physics2D.SyncTransforms();
            // Debug.Log(updatedMoveDir);
            //  Debug.Log(updatedMoveSpeed);
            myAnim.SetBool("Running", true);
            RaycastHit2D[] horizontalCheck = Physics2D.BoxCastAll(groundCollider.bounds.center, groundCollider.bounds.size - Vector3.one * .01f, 0f, updatedMoveDir, updatedMoveSpeed * Time.fixedDeltaTime, groundMask);

            bool canMoveDirection = true;

            if (horizontalCheck.Length > 0)
            {
                //   Debug.Log(horizontalCheck);
                float travelDistance = updatedMoveSpeed * Time.fixedDeltaTime;
                for (int i = 0; i < horizontalCheck.Length; i++)
                {

                    if (Mathf.Abs(horizontalCheck[i].normal.x) > .5f && ((updatedMoveDir.x > 0 && horizontalCheck[i].point.x > transform.position.x) || (updatedMoveDir.x < 0 && horizontalCheck[i].point.x < transform.position.x)))
                    {
                        // Debug.DrawRay(horizontalCheck[i].point, horizontalCheck[i].normal, Color.blue);
                        //    Debug.Log("reee");

                        travelDistance = horizontalCheck[i].distance;
                        //                        Debug.Log(travelDistance);
                        canMoveDirection = false;
                        break;
                    }

                }
                if (canMoveDirection)
                {
                    transform.position += updatedMoveDir * travelDistance;
                }
                else
                {
                    transform.position += updatedMoveDir * travelDistance;
                }
            }
            else
            {
                transform.position += updatedMoveDir * updatedMoveSpeed * Time.fixedDeltaTime;
            }
            Physics2D.SyncTransforms();
            RaycastHit2D[] verticalCheck;
            Vector2 verticalCheckPoint;
            if (transform.localScale.x > 0)
            {
                verticalCheckPoint = groundCollider.bounds.max;
                verticalCheck = Physics2D.RaycastAll(verticalCheckPoint, Vector2.down, groundCollider.size.y, groundMask);
                for (int i = 0; i < verticalCheck.Length; i++)
                {
                    if (Mathf.Abs(verticalCheck[i].normal.x) > 0f && ((updatedMoveDir.x > 0 && verticalCheck[i].point.x > transform.position.x) || (updatedMoveDir.x < 0 && verticalCheck[i].point.x < transform.position.x)))
                    {
                        transform.position += Vector3.up * (groundCollider.size.y - verticalCheck[i].distance);
                        Physics.SyncTransforms();
                        break;
                    }
                }

            }
            else
            {
                verticalCheckPoint = new Vector2(groundCollider.bounds.min.x, groundCollider.bounds.max.y);
                verticalCheck = Physics2D.RaycastAll(verticalCheckPoint, Vector2.down, groundCollider.size.y, groundMask);
                for (int i = 0; i < verticalCheck.Length; i++)
                {
                    if (Mathf.Abs(verticalCheck[i].normal.x) > 0f && ((updatedMoveDir.x > 0 && verticalCheck[i].point.x > transform.position.x) || (updatedMoveDir.x < 0 && verticalCheck[i].point.x < transform.position.x)))
                    {
                        transform.position += Vector3.up * (groundCollider.size.y - verticalCheck[i].distance);
                        Physics.SyncTransforms();
                        break;
                    }
                }

            }

        }
        else
        {
            myAnim.SetBool("Running", false);
        }
    }

    public void VerticalMove()
    {
        customGravity += (Physics2D.gravity.y * Physics2D.gravity.y) * .5f * Time.fixedDeltaTime;
        if (customGravity > maxGravity)
        {
            customGravity = maxGravity;
        }
        Physics2D.SyncTransforms();
        RaycastHit2D[] fallingCheck = Physics2D.BoxCastAll(groundCollider.bounds.center, groundCollider.bounds.size, 0f, Vector2.down, customGravity * Time.fixedDeltaTime, groundMask);
        // need bool to track if we hit a ground;
        if (fallingCheck.Length > 0)
        {
            bool changePosition = true;
            Vector3 fallingVector = Vector3.down * customGravity * Time.fixedDeltaTime;
            for (int i = 0; i < fallingCheck.Length; i++)
            {
                if (Mathf.Abs(fallingCheck[i].normal.x) < 1 && fallingCheck[i].normal.y > 0)
                {
                    float yPos = (groundCollider.bounds.center - Vector3.up * groundCollider.bounds.extents.y).y;
                    float yDif = yPos - fallingCheck[i].point.y;
                    transform.position += Vector3.down * yDif;
                    transform.SetParent(fallingCheck[i].collider.transform);
                    grounded = true;
                    myAnim.SetBool("Grounded", true);

                    doubleJumpOnCooldown = false;
                    isJumping = false;
                    customGravity = 0f;
                    changePosition = false;
                    break;
                }
                else
                {
                    //  transform.position += Vector3.down * customGravity * Time.fixedDeltaTime;
                }
            }
            if (changePosition)
            {
                transform.position += fallingVector;
            }
        }
        else
        {
            transform.position += Vector3.down * customGravity * Time.fixedDeltaTime;
        }
    }



    public void JumpCheck()
    {
        jumpTracker++;
        RaycastHit2D[] verticalCheck = Physics2D.BoxCastAll(groundCollider.bounds.center, groundCollider.bounds.size, 0f, Vector3.up, jumpHeight * Time.fixedDeltaTime, groundMask);
        if (verticalCheck.Length > 0)
        {
            bool canMoveUp = true;
            for (int i = 0; i < verticalCheck.Length; i++)
            {
                if (verticalCheck[i].collider.gameObject.GetComponent<PlatformEffector2D>() == null && verticalCheck[i].normal.y < 0)
                {
                    canMoveUp = false;
                    transform.position += Vector3.up * verticalCheck[i].distance;
                }
            }
            if (canMoveUp)
            {
                transform.position += Vector3.up * jumpHeight * Time.fixedDeltaTime;
            }
        }
        else
        {
            transform.position += Vector3.up * jumpHeight * Time.fixedDeltaTime;
        }
    }

    public void ColliderCheck()
    {
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCapsuleAll(bodyCollider.bounds.center, bodyCollider.size, bodyCollider.direction, 0f, collisionMask));
        if (colliders.Count > 0)
        {
            for(int i = 0; i < colliders.Count;i++){
                if(colliders[i].gameObject.CompareTag("Toxic")){
                    Kill();
                } else if(colliders[i].gameObject.CompareTag("Enemy")){
                    Kill();
                } 
            }


        }
    }

    public bool CheckGrounded()
    {
        if (grounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DisableSprite()
    {
        sprite.enabled = false;
    }

    public void Hide(GameObject newHidingObject)
    {
        myAnim.SetBool("Crouching",true);
        bodyCollider.enabled = false;
        groundCollider.enabled = false;
        flowerHolderTransform.localPosition = new Vector3(.02f,2.15f,0f);
        lineRenderer.transform.localPosition = new Vector3(.02f,1.5f,0f);
        Color newColor = sprite.color;
        newColor.a = .5f;
        sprite.color = newColor;
        flowerSprite.color = newColor;
        lineRenderer.material.SetInt("_Hiding",1);
        //stemMaterial.SetInt("_Hiding",1);
        hidingObject = newHidingObject;
        transform.position = hidingObject.GetComponent<HidingObject>().hidePosition.position;
        playerState = PlayerStates.hiding;
    }

    public void Appear(){
        myAnim.SetBool("Crouching",false);
        bodyCollider.enabled = true;
        groundCollider.enabled = true;
        flowerHolderTransform.localPosition = new Vector3(.02f,2.48f,0f);
        lineRenderer.transform.localPosition = new Vector3(.02f,1.83f,0f);
        Color newColor = sprite.color;
        newColor.a = 1f;
        sprite.color = newColor;
        flowerSprite.color = newColor;
        lineRenderer.material.SetInt("_Hiding",0);
        //stemMaterial.SetInt("_Hiding",0);
        transform.position = hidingObject.GetComponent<HidingObject>().appearPosition.position;
        hidingObject = null;
        playerState = PlayerStates.normal;
    }



    public void Capture(){
        playerState = PlayerStates.restricted;
        stemController.Retract();
    }

    public void Release(){
        playerState = PlayerStates.normal;
    }

    public void Plant(){
        playerState = PlayerStates.planted;
        myAnim.SetBool("Crouching",true);
        flowerHolderTransform.localPosition = new Vector3(.02f,2.15f,0f);
        lineRenderer.transform.localPosition = new Vector3(.02f,1.5f,0f);

    }

    public void Uproot(){
        playerState = PlayerStates.normal;
        myAnim.SetBool("Crouching",false);
        flowerHolderTransform.localPosition = new Vector3(.02f,2.48f,0f);
        lineRenderer.transform.localPosition = new Vector3(.02f,1.83f,0f);
    }

    public void Reset(){
        playerState = PlayerStates.normal;
        myAnim.Rebind();
        stemController.Reset();
    }

    public void Kill(){
        checkpoint.Reset();
    }
}
