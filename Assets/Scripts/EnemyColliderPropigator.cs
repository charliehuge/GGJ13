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
		Init ();
	}
	
	public void Init()
	{
		if(playerEntity == null)
			playerEntity = EntityController.GetInstance().playerEntity;
		if(playerObj == null)
			playerObj = playerEntity.gameObject;
	}
	
	void OnTriggerEnter (Collider collisionInfo)
	{
		Init ();
		
		if(collisionInfo.gameObject == playerObj){
			if(CollisionPropigate != null)
				CollisionPropigate(true);
		}
		//Debug.Log ("Collided with : " + collisionInfo.gameObject.name);
	}
	
	void OnTriggerExit(Collider collisionInfo) 
	{
		Init ();
		
		if(collisionInfo.gameObject == playerObj){
			if(CollisionPropigate != null)
				CollisionPropigate(false);
		}
		//Debug.Log ("stopped colliding with : " + collisionInfo.gameObject.name);
	}
	/*
	void OnTriggerStay (Collider other)
	{
		if(collisionInfo.gameObject == playerObj){
			if(CollisionPropigate != null)
				CollisionPropigate(false);
		}
	}
	*/
}
