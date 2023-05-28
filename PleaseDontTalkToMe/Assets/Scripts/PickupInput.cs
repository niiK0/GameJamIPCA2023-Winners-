using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PickupInput : MonoBehaviour
{
    public GameObject pickUpVFX;
    public InputFunction keyFunctionToGive;
    public LayerMask playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != playerLayer && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHeadset")) return;
        if (UI_Manager.instance.occupiedSlots >= UI_Manager.instance.keyUISlots.Length) return;

        var playerInput = collision.GetComponent<InputHolder>();

        if (playerInput)
        {
            playerInput.AddInputObject(keyFunctionToGive);
            var vfx = Instantiate(pickUpVFX,transform.position,Quaternion.identity);
            Destroy(vfx, 2.0f);
            Destroy(gameObject);
        }

    }
}
