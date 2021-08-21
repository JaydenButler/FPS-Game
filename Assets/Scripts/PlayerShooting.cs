using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private Transform gunBarrell;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //Shoot!
                ShootServerRpc();
            }
        }
    }
    
    //These run on the server, called by client .:. client --> server
    [ServerRpc]
    void ShootServerRpc()
    {
        //Do the raycast checks on the server to see if he hit an enemy
        if (Physics.Raycast(gunBarrell.position, gunBarrell.forward, out RaycastHit hit, 200f))
        {
            var enemyHealth = hit.transform.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }
        }
        
        ShootClientRpc();
    }
    
    //These run on the client, called by server .:. server --> client
    [ClientRpc]
    void ShootClientRpc()
    {
        var gunBarrellPosition = gunBarrell.position;
        var bullet = Instantiate(bulletTrail, gunBarrellPosition, Quaternion.identity);
        bullet.AddPosition(gunBarrellPosition);
        if (Physics.Raycast(gunBarrellPosition, gunBarrell.forward, out RaycastHit hit, 200f))
        {
            bullet.transform.position = hit.point;
        }
        else
        {
            bullet.transform.position = gunBarrellPosition + (gunBarrell.forward * 200f);
        }
    }
}