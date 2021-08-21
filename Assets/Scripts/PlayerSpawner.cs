using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerSpawner : NetworkBehaviour
{
    private CharacterController _cc;
    private Renderer[] _renderers;
    [SerializeField] private Behaviour[] scripts;
    
    // Start is called before the first frame update
    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer && Input.GetKeyDown(KeyCode.Y))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        RespawnServerRpc();
    }
    
    //These run on the server, called by client .:. client --> server
    [ServerRpc]
    void RespawnServerRpc()
    {
        RespawnClientRpc(GetRandomSpawn());
    }
    
    //These run on the client, called by server .:. server --> client
    [ClientRpc]
    void RespawnClientRpc(Vector3 spawnPos)
    {
        StartCoroutine(RespawnCoroutine(spawnPos));
    }
    
    private Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 1f, z);
    }

    IEnumerator RespawnCoroutine(Vector3 spawnPos)
    {
        _cc.enabled = false;
        SetPlayerState(false);
        transform.position = spawnPos;
        yield return new WaitForSeconds(0.5f);
        _cc.enabled = true;
        SetPlayerState(true);
    }
    
    void SetPlayerState(bool state)
    {
        foreach (var script in scripts)
        {
            script.enabled = state;
        }

        foreach (var renderer in _renderers)
        {
            renderer.enabled = state;
        }
    }
}
