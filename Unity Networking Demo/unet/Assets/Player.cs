using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    
    //this variable will be synced across all clients and the server
    [SyncVar]
    public int healthPoints;


    // Use this for initialization
    void Start () {
		
	}

    //runs after the player object has been created.
    public override void OnStartLocalPlayer()
    {
        
        this.GetComponentInChildren<TextMesh>().color = Color.red; //Set own text to red.
    }

    // Update is called once per frame
    void Update() {
        //always set the text mesh to the health.
        this.GetComponentInChildren<TextMesh>().text = healthPoints.ToString();

        // if we are server, we can send out RPC calls to all clients.
        if (isServer && Input.GetKeyDown("l"))
        {
            RpcSetColor(Color.blue); //invokes the function RpcSetColor on all ready clients.
        }


        if (!isLocalPlayer) //stuff below here should only be executed on the gameobject that belongs to the person who's running the program
        {
            return;
        }

        //as a client, we can manipulate our own (synced) member variables and they will be propagated to the other clients.
        if (Input.GetKeyDown("k"))
        {
            CmdSendHealth(Random.Range(0, 100)); //CMD function, called by client, executed on server.
        }

        // Basic controlls.
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }


    [Command] //function executed on the server, called by client
    void CmdSendHealth(int value)
    {
        healthPoints = value;        
	}

    [ClientRpc] //function executed on all clients, called by server.
    void RpcSetColor(Color c)
    {
        GetComponent<Renderer>().material.color = c;
    }
}

/*
Allowed arguments:
basic types (byte, int, float, string, UInt64, etc)
arrays of basic types
structs containing allowable types
built-in unity math types (Vector3, Quaternion, etc)
NetworkIdentity
NetworkInstanceId
NetworkHash128
GameObject with a NetworkIdentity component attached
*/