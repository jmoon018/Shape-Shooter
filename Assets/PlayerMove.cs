using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	void updateRotation() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast(ray, out hit);
		if (hit.collider.gameObject.tag == "Terrain")
		{
			Vector3 __target = hit.point;
			//__target.y += 0.5f; // a little above ground
			//transform.localRotation = new Quaternion(0, 0, __target.z, 1);
			transform.LookAt(__target);
			Debug.Log("rotating around ... " + __target);
		}
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

	//	transform.Translate(x, z, 0);
		transform.position = new Vector3(transform.position.x + x, transform.position.y + z, 0);
		updateRotation();
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
			transform.position + transform.forward,
			Quaternion.identity);

		// make the bullet move away in front of the player
		bullet.GetComponent<Rigidbody>().velocity = transform.forward * 10;//new Vector3(1, 1, 0) * 4;//-transform.forward * 40;

		// spawn the bullet on the clients
		NetworkServer.Spawn(bullet);

		// make bullet disappear after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
