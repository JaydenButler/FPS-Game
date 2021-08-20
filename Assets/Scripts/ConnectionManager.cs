using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private GameObject connectionButtonPanel;
    
    //Server only
    public void Host()
    {
        connectionButtonPanel.SetActive(false);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(GetRandomSpawn(), Quaternion.identity);
    }
    //Server only
    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        //Check incoming data
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password1234";
        
        //Uses default prefab hash because it's null. Can use this to make different prefabs spawn
        callback(true, null, approve, GetRandomSpawn(), Quaternion.identity);
    }

    public void Join()
    {
        connectionButtonPanel.SetActive(false);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password1234");
        NetworkManager.Singleton.StartClient();
    }

    private Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0, z);
    }
}
