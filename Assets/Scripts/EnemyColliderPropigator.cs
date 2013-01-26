using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyColliderPropigator : MonoBehaviour 
{
	public Action<bool> CollisionPropigate;
	
	void OnCollisionEnter(Collision collisionInfo)
	{
		if(collisionInfo.gameObject.tag.Contains("Player")){
			CollisionPropigate(true);
		}
	}
	
	void OnCollisionExit(Collision collisionInfo) 
	{
		if(collisionInfo.gameObject.tag.Contains("Player")){
			CollisionPropigate(false);
		}
	}
}
