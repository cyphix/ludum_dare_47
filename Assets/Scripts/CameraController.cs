using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraController : MonoBehaviour
{
    public Transform targetTransform;

    public Transform farBackground = null;
    public float farBackgroundMoveRate = 1.0f;
    public Transform middleBackground = null;
    public float middleBackgroundMoveRate = 0.5f;
    public Transform closeBackground = null;
    public float closeBackgroundMoveRate = 0.25f;
    public bool turnOffY = false;

    public float followRateX = 0.1f;
    public float followRateY = 0.05f;

    private float lastUpdateXPos = 0.0f;
    private float offsetX;
    private float offsetY;
    private Vector3 velocityX = Vector3.zero;
    private Vector3 velocityY = Vector3.zero;

    void Start()
    {
        this.offsetX = transform.position.x - this.targetTransform.position.x;
        this.offsetY = transform.position.y - this.targetTransform.position.y;
    }

    void Awake()
    {
        this.offsetX = transform.position.x - this.targetTransform.position.x;
        this.offsetY = transform.position.y - this.targetTransform.position.y;
    }
    
    void Update()
    {
        if(this.farBackground != null)
        {
            this.farBackground.position += new Vector3(
                (targetTransform.position.x - this.lastUpdateXPos) * this.farBackgroundMoveRate, 0.0f, 0.0f
            );
        }
        if (this.middleBackground != null)
        {
            this.middleBackground.position += new Vector3(
                (targetTransform.position.x - this.lastUpdateXPos) * this.middleBackgroundMoveRate, 0.0f, 0.0f
            );
        }
        if (this.closeBackground != null)
        {
            this.closeBackground.position += new Vector3(
                (targetTransform.position.x - this.lastUpdateXPos) * this.closeBackgroundMoveRate, 0.0f, 0.0f
            );
        }
    }

    void LateUpdate()
    {
        //transform.position = new Vector3(this.targetTransform.position.x, transform.position.y, transform.position.z);

        // X
        transform.position = Vector3.SmoothDamp(
               transform.position,
               new Vector3(this.targetTransform.position.x + this.offsetX, transform.position.y, transform.position.z),
               ref this.velocityX,
               this.followRateX
        );

        // Y
        if (!this.turnOffY)
        {
            transform.position = Vector3.SmoothDamp(
                   transform.position,
                   new Vector3(transform.position.x, this.targetTransform.position.y + this.offsetY, transform.position.z),
                   ref this.velocityY,
                   this.followRateY
            );
        }
    }
}
