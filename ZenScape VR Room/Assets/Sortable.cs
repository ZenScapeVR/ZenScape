using UnityEngine;
using TMPro;

public class Sortable : MonoBehaviour
{
    public Material[] materials; // Array of materials for color
    public string[] names; // Array of names
    private MeshRenderer meshRenderer;
    private TextMeshPro nameText;
    private Rigidbody rb;
    // Property to access the chosen name
    public string ChosenName { get; private set; }

    void Start()
    {
        // Get components
        meshRenderer = GetComponent<MeshRenderer>();
        nameText = GetComponentInChildren<TextMeshPro>();

        // Randomly choose material and name
        int materialIndex = Random.Range(0, materials.Length);
        int nameIndex = Random.Range(0, names.Length);

        // Apply material to cube
        if (meshRenderer != null && materials.Length > 0)
        {
            meshRenderer.material = materials[materialIndex];
        }

        // Apply name to text component
        if (nameText != null && names.Length > 0)
        {
            nameText.text = names[nameIndex];
            // Store the chosen name
            ChosenName = names[nameIndex];
        }
    }
}