using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PickupInput : MonoBehaviour
{
    public InputFunction keyFunctionToGive;
    public LayerMask playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != playerLayer) return;

        var playerInput = collision.GetComponent<InputHolder>();

        if (playerInput)
        {
            playerInput.AddInputObject(keyFunctionToGive);
            Destroy(gameObject);
        }

    }
}
