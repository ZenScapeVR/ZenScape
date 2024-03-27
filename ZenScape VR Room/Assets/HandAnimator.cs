using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;
    private Animator anim;
        
    void Start()
    {
        // Get the Animator component attached to the same GameObject as this script
        anim = GetComponent<Animator>();
        
        // Check if Animator component is found
        if (anim == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();

        // Check if anim is not null before using it
        if (anim != null)
        {
            anim.SetFloat("Trigger", triggerValue);
            anim.SetFloat("Grip", gripValue);
        }
    }
}
