using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TypingButton : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource audioSource;
    public AudioClip click;

    private bool isPressed;

    public TextMeshPro textMesh; // Reference to the 3D TextMeshPro object
    public char characterToAdd; // Character to add when pressed (0-9 or delete)

    Vector3 originalPosition; // Store the original position of the button

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        isPressed = false;

        // Store the original position of the button
        originalPosition = button.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            isPressed = true;
            presser = other.gameObject;
            button.transform.Translate(Vector3.down * 0.015f); // Adjust the button press depth as needed
            onPress.Invoke();
            PlaySound(click);

            // Append character to text
            if (textMesh != null)
            {
                if (characterToAdd == 'x') // Check for backspace identifier
                {
                    if (textMesh.text.Length > 0)
                    {
                        textMesh.text = textMesh.text.Substring(0, textMesh.text.Length - 1);
                    }
                }
                else
                {
                    textMesh.text += characterToAdd;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPressed && other.gameObject == presser)
        {
            isPressed = false;
            ResetButtonPosition();
            onRelease.Invoke();
        }
    }

    private void ResetButtonPosition()
    {
        button.transform.position = originalPosition;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
