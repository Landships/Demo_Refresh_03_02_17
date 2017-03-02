using UnityEngine.Networking;
using UnityEngine;
using System.Collections;


public class CustomNetworkManager : NetworkManager
{

    private GameObject tank1;
    private GameObject tank2;
    private Transform turretSpawnPos2;
    private Transform driverSpawnPos2;
    private Transform turretSpawnPos1;
    private Transform driverSpawnPos1;

    
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (numPlayers == 0)
        {
            foreach (GameObject obj in spawnPrefabs)
            {
                if (obj.name == "PlayerTank")
                {
                    tank1 = (GameObject)Instantiate(obj);
                    turretSpawnPos1 = tank1.transform.GetChild(0).GetChild(12);
                    driverSpawnPos1 = tank1.transform.GetChild(0).GetChild(13);

                    NetworkServer.Spawn(tank1);

                }
            }
            var player = (GameObject)GameObject.Instantiate(playerPrefab, driverSpawnPos1.position, driverSpawnPos1.rotation);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            player.GetComponent<Position>().parentid = tank1.GetComponent<NetworkIdentity>().netId;
            player.GetComponent<Position>().position = "DRIVER";
            return;
            /*
            var player = (GameObject)GameObject.Instantiate(playerPrefab, turretSpawnPos1.position, turretSpawnPos1.rotation);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            player.GetComponent<Position>().parentid = tank1.GetComponent<NetworkIdentity>().netId;
            player.GetComponent<Position>().position = "TURRET";
            return;*/
        }

        if (numPlayers == 1)
        {
           /* foreach (GameObject obj in spawnPrefabs)
            {
                if (obj.name == "PlayerTank")
                {
                    tank2 = (GameObject)Instantiate(obj);
                    turretSpawnPos2 = tank2.transform.GetChild(0).GetChild(12);
                    driverSpawnPos2 = tank2.transform.GetChild(0).GetChild(13);

                    NetworkServer.Spawn(tank2);

                }
            }*/
            var player = (GameObject)GameObject.Instantiate(playerPrefab, turretSpawnPos1.position, turretSpawnPos1.rotation);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            player.GetComponent<Position>().parentid = tank1.GetComponent<NetworkIdentity>().netId;
            player.GetComponent<Position>().position = "TURRET";
            return;
        }

    }
}

