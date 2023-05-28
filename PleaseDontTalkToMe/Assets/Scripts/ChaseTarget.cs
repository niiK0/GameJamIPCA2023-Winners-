using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 _startOffset;
    // Start is called before the first frame update
    void Start()
    {
        //_startOffset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position - _startOffset;
    }
}
