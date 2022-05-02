using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    // Default distance between target and player
    public Vector3 cameraOffset;

    // Smooth factor will use Cam rotation
    private float smoothFactor = 0.5f;

    // Will check that the camera looked at on the target or not
    [SerializeField]
    private bool lookAtTarget = false;


    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - target.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = target.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        // Cam Rotation
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
