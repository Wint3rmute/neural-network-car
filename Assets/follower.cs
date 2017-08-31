using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour {

	public GameObject follow;
	public float bias;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 moveCamTo = follow.transform.position - transform.forward * 10f ;

		Camera.main.transform.position = 
			Camera.main.transform.position * bias +
			moveCamTo * (1f - bias);
		
		Camera.main.transform.LookAt (follow.transform.position);

	}
}
