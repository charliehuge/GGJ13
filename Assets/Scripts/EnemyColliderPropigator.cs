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
		if(playerEntity == null){
			playerEntity = EntityController.GetInstance().playerEntity;
			if(playerEntity == null){
				Debug.LogError("Lolwut");
			}
		}
		if(playerEntity != null && playerObj == null)
				playerObj = playerEntity.gameObject;
	}
	
	void OnTriggerEnter (Collider collisionInfo)
	{
		Debug.LogError("detected player in collider");
		Init ();
		
		if(collisionInfo.gameObject == playerObj){
			if(CollisionPropigate != null)
				CollisionPropigate(true);
		}
		//Debug.Log ("Collided with : " + collisionInfo.gameObject.name);
	}
	
	void OnTriggerExit(Collider collisionInfo) 
	{
		Debug.LogError("lost player in collider");
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
