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
	
	void OnTriggerEnter (Collision collisionInfo)
	{
		if(collisionInfo.gameObject == playerObj){
			CollisionPropigate(true);
		}
	}
	
	void OnTriggerExit(Collision collisionInfo) 
	{
		if(collisionInfo.gameObject == playerObj){
			CollisionPropigate(false);
		}
	}
}
