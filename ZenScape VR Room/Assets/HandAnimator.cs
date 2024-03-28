using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;
    private Animator anim;

    // Smoothness factor for the interpolation
    [SerializeField] private float smoothnessGrip = 20f;
    [SerializeField] private float smoothnessTrigger = 25f;

    // Current values of trigger and grip
    private float currentTriggerValue;
    private float currentGripValue;
        
    void Start()
    {
        anim = GetComponent<Animator>();
        
        if (anim == null)
        {
            // Log an error message to the console if Animator component is not found
            Debug.LogError("Animator component not found!");
        }
    }

    void Update()
    {
        float targetTriggerValue = triggerAction.action.ReadValue<float>();
        float targetGripValue = gripAction.action.ReadValue<float>();

        // Smoothly interpolate the trigger and grip values
        currentTriggerValue = Mathf.Lerp(currentTriggerValue, targetTriggerValue, Time.deltaTime * smoothnessTrigger);
        currentGripValue = Mathf.Lerp(currentGripValue, targetGripValue, Time.deltaTime * smoothnessGrip);

        if (anim != null)
        {
            anim.SetFloat("Trigger", currentTriggerValue);
            anim.SetFloat("Grip", currentGripValue);
        }

        // Log the values of trigger and grip to the console
        Debug.Log("Trigger value: " + currentTriggerValue + ", Grip value: " + currentGripValue);
    }
}
