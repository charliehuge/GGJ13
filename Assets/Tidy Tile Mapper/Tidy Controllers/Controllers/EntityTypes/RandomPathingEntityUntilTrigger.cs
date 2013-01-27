using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomPathingEntityUntilTrigger : PathingEntity {
	public bool playerTriggeredMe = false; //when one of the colliders is trigered
	
	public bool followPlayerUntilEnd = false;
	public EnemyColliderPropigator[] collidersToBeTriggeredBy;  //configured by level designer
	
	public void Start()
	{
		if(collidersToBeTriggeredBy != null){
			foreach(EnemyColliderPropigator col in collidersToBeTriggeredBy){
				col.OnlyEnteredEvent += PlayerTriggeredMe;
			}
		}
	}
	
	public void PlayerTriggeredMe() 
	{
		playerTriggeredMe = true;
		PathTo(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
	}
	
	public override void OnReachPathEnd (int x, int y, int z)
	{
		//follow like a hound or wander around after going to their postiion at the time of detection
		if(playerTriggeredMe && followPlayerUntilEnd){
			//NOTE: this may need to be the open block adjacent to the player
			PathTo(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
		}else{
			RePath ();
		}
	}
	
	public void RePath(){
		Debug.Log ("REpath");
		
		List<Vector3> b = new List<Vector3>();
		
		for(int px = x - pathRadius; px <= x + pathRadius; px++){
			for(int py = y - pathRadius; py <= y + pathRadius; py++){
				if(CanWalkTo(px,py,z)){
					b.Add(new Vector3(px,py,z));
				}
			}
		}
		
		if(b.Count <= 0){
			return;
		}
		
		
		Vector3 target = b[UnityEngine.Random.Range (0,b.Count)];
		
		PathTo((int)target.x,(int)target.y,z);
	}
}
