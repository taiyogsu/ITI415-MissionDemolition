using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {
	//fields set in the UNity Inspector pane
	public GameObject prefabProjectile;
	public float velocityMult = 4f;
	public bool     ______________;
	//fields set dynamically
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;

	void Awake()
	{
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
	}


	void OnMouseEnter()
	{
		//print("Slingshot:OnMouseEnter()");
		launchPoint.SetActive(true);
	}


	void OnMouseExit()
	{
		launchPoint.SetActive(false);
		//print("Slingshot:OnMouseExit()");
	}

	void OnMouseDown()
	{
		//player has pressed mouse button while over Slingshot
		aimingMode = true;
		//instantiate a Projectile
		projectile = Instantiate(prefabProjectile) as GameObject;
		//start it at the launchPoint
		projectile.transform.position = launchPos;
		//set it to isKinematic for now

		projectile.GetComponent<Rigidbody>().isKinematic = true;
	}


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//if Slingshot is not  in aimingMode, don't run this code
		if (!aimingMode) return;
		//get the current mouse position in 2D screen coordinates
		Vector3 mousePos2D = Input.mousePosition;
		//Convert the mouse position to 3D world coordinates
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
		//Find teh delta from the launchPos to the mousePos3D
		Vector3 mouseDelta = mousePos3D - launchPos;
		// Limit mouseDelta to the raius of the Slingshot Sphere Collider
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		if (mouseDelta.magnitude > maxMagnitude)
		{
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
		}
		//Move the projectile to this new position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;

		if (Input.GetMouseButtonUp(0))
		{
			//the mouse has been released
			aimingMode = false;

			projectile.GetComponent<Rigidbody>().isKinematic = false;
			projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
			projectile = null;
		}

	}
}
