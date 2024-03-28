using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RespawnOnDrop : MonoBehaviour
{
    private Vector3 originalPosition;
    private XRGrabInteractable grabInteractable;
    private double minObjectHeight = .5;
    void Start()
    {
        // Store the original position of the object
        originalPosition = transform.position;

        // Get reference to XRGrabInteractable component
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // Check if the Y position is less than what we can reach
        if (transform.position.y <= minObjectHeight)
        {
            // Move the object back to its original position
            transform.position = originalPosition;
        }
    }
}
