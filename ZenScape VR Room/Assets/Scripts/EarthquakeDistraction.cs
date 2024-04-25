using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeDistraction : MonoBehaviour
{
    public bool start = false;
    [SerializeField] private float duration = 1;
    [SerializeField] private Camera camera;
    public AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = camera.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/duration);
            camera.transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        camera.transform.position = startPosition;
    }
}
