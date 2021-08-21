using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PlayerHealth : NetworkBehaviour
{
    //Make a sync'd var to store health
    [SerializeField] private NetworkVariableInt health = new NetworkVariableInt(new NetworkVariableSettings{ WritePermission = NetworkVariablePermission.OwnerOnly}, 100);

    private PlayerSpawner _playerSpawner;

    private void Start()
    {
        _playerSpawner = GetComponent<PlayerSpawner>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (health.Value <= 0 && IsLocalPlayer)
        {
            health.Value = 100;
            _playerSpawner.Respawn();
        }
    }
    
    //Runs on the server
    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }
}
