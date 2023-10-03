using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPos : MonoBehaviour
{
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) 
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = target.position;
    }
}
