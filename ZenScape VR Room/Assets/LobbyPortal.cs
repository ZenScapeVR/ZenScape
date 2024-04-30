using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPortal : MonoBehaviour
{
    // Name of the scene to load
    public string lobbySceneName = "Lobby";

    // Method to load the lobby scene

    void Start()
    {
        DisablePortal();
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene(lobbySceneName);
    }

    // Enable the portal
    public void EnablePortal()
    {
        gameObject.SetActive(true);
    }

    // Disable the portal
    public void DisablePortal()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger collider
        if (other.CompareTag("Player"))
        {
            // Load the lobby scene
            LoadLobbyScene();
        }
    }
}
