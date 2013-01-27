using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyColliderPropigator : MonoBehaviour 
{
	public Entity playerEntity;
	public GameObject playerObj;
	public Action<bool> CollisionPropigate;
	
	void Start()
	{
		playerEntity = EntityController.GetInstance().playerEntity;
		playerObj = playerEntity.gameObject;
	}
	
	void OnTriggerEnter (Collider collisionInfo)
	{
		if(collisionInfo.gameObject == playerObj){
			CollisionPropigate(true);
		}
		//Debug.Log ("Collided with : " + collisionInfo.gameObject.name);
	}
	
	void OnTriggerExit(Collider collisionInfo) 
	{
		if(collisionInfo.gameObject == playerObj){
			CollisionPropigate(false);
		}
		//Debug.Log ("stopped colliding with : " + collisionInfo.gameObject.name);
	}
}
