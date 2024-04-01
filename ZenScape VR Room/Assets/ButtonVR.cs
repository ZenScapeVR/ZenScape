using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource audioSource;
 	public AudioClip click;

    private bool isPressed;

    public string sceneToLoad; // Name of the scene to load

    void Start()
    {
 		audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        isPressed = false;
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
            // Load the next scene after a delay
            StartCoroutine(LoadNextScene());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPressed && other.gameObject == presser)
        {
            isPressed = false;
            button.transform.Translate(Vector3.up * 0.03f); // Adjust the button release depth as needed
            onRelease.Invoke();
        }
    }

    IEnumerator LoadNextScene()
    {
        // Wait for a short delay before loading the next scene
        yield return new WaitForSeconds(1.0f); // Adjust the delay as needed

        // Load the next scene
        SceneManager.LoadScene(sceneToLoad);
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