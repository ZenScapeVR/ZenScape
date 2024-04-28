using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceDistraction : MonoBehaviour
{

    public GameObject ambulance;
    [SerializeField] private GameObject start_position;
    [SerializeField] private GameObject end_position;
    [SerializeField] private AudioSource siren;
    [SerializeField] private GameObject this_ambulance;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        //StartEvent();
        //Instantiate(ambulance, start_position.transform.position, Quaternion.identity);
        //StartCoroutine(Move(ambulance, end_position, speed));
    }

    // Update is called once per frame
    void Update()
    {
        if(this_ambulance != null)
        {
            if (this_ambulance.transform.position == end_position.transform.position)
                EndEvent();
        }
        
    }

    public void StartEvent()
    {
        this_ambulance = Instantiate(ambulance, start_position.transform.position, start_position.transform.rotation);
        ambulance.GetComponent<AudioSource>().Play();
        StartCoroutine(Move(this_ambulance, end_position, speed));
    }

    void EndEvent()
    {
        this_ambulance.GetComponent<AudioSource>().Stop();
        Destroy(this_ambulance);
    }

    IEnumerator Move(GameObject objectA, GameObject objectB, float speedTranslation)
    {
        while(objectA.transform.position != objectB.transform.position)
        {
            objectA.transform.position = Vector3.MoveTowards(objectA.transform.position, objectB.transform.position, speedTranslation * Time.deltaTime);
            yield return null;
        }
    }
}
