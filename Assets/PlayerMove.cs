using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () { 

		if (!isLocalPlayer) {
			return;
		}

		var x = Input.GetAxis("Horizontal") * 0.1f;
		var z = Input.GetAxis("Vertical") * 0.1f;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}

		transform.Translate(x, z, z);
	}

	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.red;
	}

	[Command]
	void CmdFire()
	{
		Debug.Log(transform.forward);
		// create the bullet object from the bullet prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			transform.position - transform.forward,
			Quaternion.identity);

		// make the bullet move away in front of the player
		bullet.GetComponent<Rigidbody>().velocity = new Vector3(1, 1, 0) * 4;//-transform.forward * 40;

		// spawn the bullet on the clients
		NetworkServer.Spawn(bullet);

		// make bullet disappear after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
