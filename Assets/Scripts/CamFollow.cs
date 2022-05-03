using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // VARIABLES
    [SerializeField]
    private Transform playerTarget;

    // Default distance between target and player
    public Vector3 cameraOffset;

    // Smooth factor will use Cam rotation
    [SerializeField]
    private float smoothFactor = 0.5f;

    // Will check that the camera looked at on the target or not
    [SerializeField]
    private bool lookAtTarget = false;

    [SerializeField]
    private bool rotateAroundPlayer = true;

    [SerializeField]
    private float rotationSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - playerTarget.transform.position;
    }

    private void LateUpdate()
    {
        if (rotateAroundPlayer)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            cameraOffset = camTurnAngle * cameraOffset;
        }

        Vector3 newPosition = playerTarget.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        // Cam Rotation
        if (lookAtTarget || rotateAroundPlayer)
        {
            transform.LookAt(playerTarget);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}// CODE END
