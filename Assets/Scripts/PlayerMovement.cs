using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //VARIABLES

    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private Camera myCam;

    private Vector3 movement;
    private Rigidbody rb;
    private float speedEQ;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //Input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Turning
        movement = new Vector3(horizontalInput, 0, verticalInput);
        movement = myCam.transform.TransformDirection(movement);

        if((horizontalInput > 0.1f) || (verticalInput > 0.1f) || (horizontalInput < -0.1f) || (verticalInput < -0.1f))
        {
            var rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
        }

        // Moving
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

    }


} // CODE END
