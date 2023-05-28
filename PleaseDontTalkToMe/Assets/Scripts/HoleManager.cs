using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.gameObject.GetComponent<EnemyMovement>() != null)
            {
                collision.gameObject.GetComponent<EnemyMovement>().EnterReturningState();
            }
        }
    }
}
