using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Vector2 TeleportToPosition = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            collision.collider.transform.position = TeleportToPosition;
            ServerSend.TeleportPlayer(player.ID, TeleportToPosition);
        }
    }
}
