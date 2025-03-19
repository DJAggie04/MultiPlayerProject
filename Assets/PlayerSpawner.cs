using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject redPrefab;
    public GameObject bluePrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject playerPrefab = clientId == 0 ? redPrefab : bluePrefab;
        GameObject player = Instantiate(playerPrefab, GetSpawnPosition(clientId), Quaternion.identity);

        // Spawn with ownership
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    private Vector3 GetSpawnPosition(ulong clientId)
    {
        return clientId == 0 ? new Vector3(40, 19, 0) : new Vector3(20, 19, 0);
    }
}
