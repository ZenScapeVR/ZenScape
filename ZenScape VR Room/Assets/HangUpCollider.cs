using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangUpCollider : MonoBehaviour
{
    public PhoneCallTask phone; // Assign the phone object in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the phone
        if (other.CompareTag("PhoneHead"))
        {
            // Check if the phone object is assigned
            if (phone != null)
            {
                // Restart the phone ringing sound
                phone.HandleAction(true);
            }
            else
            {
                Debug.LogError("Phone object is not assigned in the HangUpCollider script!");
            }
        }
    }
}
