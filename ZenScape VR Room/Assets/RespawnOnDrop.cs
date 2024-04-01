using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RespawnOnDrop : MonoBehaviour
{
    private Vector3 originalPosition;
    private XRGrabInteractable grabInteractable;
    private double minObjectHeight = .2;
	private Quaternion originalRotation; // Variable to store initial rotation

    private Rigidbody rb;

    void Start()
    {

		// Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
		
        // Store the original position of the object
        originalPosition = transform.position;
		originalRotation = transform.rotation;

        // Get reference to XRGrabInteractable component
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // Check if the Y position is less than what we can reach
        if (transform.position.y <= minObjectHeight)
        {
            // Move the object back to its original position
			 // Check if Rigidbody component is not null
        	if (rb != null)
        	{
            // Set angular velocity to zero
           		rb.angularVelocity = Vector3.zero;
				rb.velocity = Vector3.zero;
			}
			transform.rotation = originalRotation;
            transform.position = originalPosition;
        }
    }
}
