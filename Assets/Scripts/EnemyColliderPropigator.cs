using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class EnemyColliderPropigator : MonoBehaviour 
{
	public Entity playerEntity;
	public GameObject playerObj;
	public Action<bool> CollisionPropigate;
	public event Action OnlyEnteredEvent;
	public bool broadcastToAllRandomPathers = false;
	
	void Start()
	{
		Init ();
	}
	
	public void Init()
	{
		if(playerEntity == null){
			playerEntity = EntityController.GetInstance().playerEntity;
			if(playerEntity == null){
				//Debug.LogError("Lolwut");
			}
		}
		if(playerEntity != null && playerObj == null)
				playerObj = playerEntity.gameObject;
	}
	
	void OnTriggerEnter (Collider collisionInfo)
	{
		//Debug.LogError("detected player in collider");
		Init ();
		
		if(collisionInfo.gameObject == playerObj){
			if(CollisionPropigate != null)
				CollisionPropigate(true);
			if(OnlyEnteredEvent != null){
				OnlyEnteredEvent();
			}
			if(broadcastToAllRandomPathers){
				Object[] randomPathers = GameObject.FindObjectsOfType(typeof(RandomPathingEntityUntilTrigger));
				if(randomPathers != null && randomPathers.Length > 0){
					foreach(Object pather in randomPathers){
						RandomPathingEntity mypather = pather as RandomPathingEntityUntilTrigger;
						mypather.PlayerTriggeredMe();
					}
				}
			}
		}
		//Debug.Log ("Collided with : " + collisionInfo.gameObject.name);
	}
	
	void OnTriggerExit(Collider collisionInfo) 
	{
		//Debug.LogError("lost player in collider");
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
