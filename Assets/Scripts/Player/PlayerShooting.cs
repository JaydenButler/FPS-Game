using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerShooting : NetworkBehaviour
{
    // [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private Rigidbody bulletRigidBody;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private float fireSpeed = 0.15f;
    [SerializeField] private Camera _camera;
    private bool _allowFire = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetButton("Fire1") && _allowFire && !FirstPersonController.Paused)
            {
                StartCoroutine(Fire());
            }
        }
    }
    
    //These run on the server, called by client .:. client --> server
    [ServerRpc]
    void ShootServerRpc()
    {
        //Do the raycast checks on the server to see if he hit an enemy
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out RaycastHit hit, 200f))
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
        var gunBarrelPosition = gunBarrel.position;
        // var bullet = Instantiate(bulletTrail, gunBarrelPosition, Quaternion.identity);
        // bullet.AddPosition(gunBarrelPosition);
        var bullet = Instantiate(bulletRigidBody, gunBarrelPosition, _camera.transform.rotation);
        var newRotation = Quaternion.LookRotation(-bullet.transform.forward, Vector3.right);
        bullet.rotation = Quaternion.Slerp(bullet.rotation, newRotation, 1f);
        bullet.velocity = gunBarrel.forward * bulletSpeed;
    }
    
    IEnumerator Fire()
    {
        _allowFire = false;
        ShootServerRpc();
        yield return new WaitForSeconds(fireSpeed);
        _allowFire = true;
    }
}