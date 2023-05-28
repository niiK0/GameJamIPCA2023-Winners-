using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentAtStart : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
    }
}
