using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public int MaxPlayers = 4;
    public int Port = 9009;

    public GameObject NetworkedPlayerPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(MaxPlayers, Port);
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(NetworkedPlayerPrefab, Vector2.zero, Quaternion.identity).GetComponent<Player>();
    }
}
