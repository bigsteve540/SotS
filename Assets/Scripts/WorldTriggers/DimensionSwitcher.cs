using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dimension
{
    none = -1,
    overworld,
    depths = 11
}

public class DimensionSwitcher : MonoBehaviour
{
    public Dimension SwitchTo = Dimension.none;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            collision.collider.transform.position = new Vector3(0, 0, (int)SwitchTo);
            Server.Clients[player.ID].CurrentDimension = SwitchTo;
            ServerSend.ShiftDimension(player.ID, (int)SwitchTo);

            foreach (Client client in Server.Clients.Values)
            {
                if(client.Player != null && client.CurrentDimension != SwitchTo)
                {
                    ServerSend.DespawnPlayer(player.ID, client.ID);
                }
            }

            foreach (Client client in Server.Clients.Values)
            {
                if (client.Player != null && client.CurrentDimension != SwitchTo)
                {
                    ServerSend.DespawnPlayer(client.ID, player.ID);
                }
            }

            foreach (Client client in Server.Clients.Values)
            {
                if (client.Player != null)
                {
                    if (client.ID != player.ID && Server.Clients[player.ID].CurrentDimension == client.CurrentDimension)
                    {
                        ServerSend.SpawnPlayer(player.ID, client.Player);
                    }
                }
            }

            foreach (Client client in Server.Clients.Values)
            {
                if (client.Player != null && Server.Clients[player.ID].CurrentDimension == client.CurrentDimension)
                {
                    ServerSend.SpawnPlayer(client.ID, player);
                }
            }
            //spawn players back into the game
        }
    }
}
