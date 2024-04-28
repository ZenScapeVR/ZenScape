using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TVStaticManager : MonoBehaviour
{
    [SerializeField] private TVStatic tv1;
    [SerializeField] private TVStatic tv2;
    [SerializeField] private TVStatic tv3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEvent()
    {
        int tv_selection = Random.Range(1, 3);
        switch (tv_selection)
        {
            case 1:
                tv1.StartEvent();
                break;
            case 2:
                tv2.StartEvent();
                break;
            case 3:
                tv3.StartEvent();
                break;
        }
    }
}
