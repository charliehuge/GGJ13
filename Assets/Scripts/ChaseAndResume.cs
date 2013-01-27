using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//chase and then random path, don't resume old path?
//TODO: make sure that oldPath[oldPath.count -1] is the end of the old path
public class ChaseAndResume : PathingEntity {
	public EnemyColliderPropigator playerDetector;
	
	float lastTimeDetectedPlayer;
	bool playerInSensor;
	bool chasingPlayer;
	
	public Vector3? targetBeforeChasingPlayer;
	
	void Start () {
		playerDetector.CollisionPropigate = OnDetectedPlayer;
	}
	
	public override void OnReachPathEnd (int x, int y, int z)
	{
		RandomPathing ();
	}
	
	public override void OnInitializeEntity ()
	{
		playerDetector.CollisionPropigate = OnDetectedPlayer;
		
		base.OnInitializeEntity();
		playerDetector.Init();
		if(chasingPlayer == false){
			RandomPathing();
		}
		//PathTo(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
	}
	
	void OnDetectedPlayer(bool foundPlayer)
	{
		lastTimeDetectedPlayer = Time.time;
		playerInSensor = foundPlayer;
		chasingPlayer = true;
		
		Vector3 lastPlayerMapIndex = new Vector3(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
		Debug.Log ("Player index: " + lastPlayerMapIndex);
		
		if(foundPlayer){
			targetBeforeChasingPlayer = targetPosition; //TODO: make sure this is always from 
			if(chasingPlayer == false){
				if(currentPath.Count == 0){
					targetBeforeChasingPlayer = null;
				}else{
					targetBeforeChasingPlayer = currentPath[currentPath.Count -1];
				}
			}
			
			Debug.LogError ("Found player");
			//seek out player for ... pinging
			//Debug.Log ("block next to player: " + findOpenBlockAdjacentToPlayer());
		}else{
			Debug.LogError ("lost player");
			//we lost player, wander around again, maybe after a delay
		}
		
		//NOTE: this may need to be the open block adjacent to the player
		PathTo(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
	}
	
	//public override void ProcessPathing()
	public void ProcessPathing()
	{
		//base.ProcessPathing();
		if(playerInSensor == false && lastTimeDetectedPlayer < Time.time - 3f){
				chasingPlayer = false; //stop chasing them if we haven't seen them in 3 seconds
				//PathTo(targetBeforeChasingPlayer);  //swap back the regular patrol path end
			}
		//if it's been 3 seconds since we've seen the player and/or they're still in our line of sight
		if(chasingPlayer &&  (playerInSensor == false && lastTimeDetectedPlayer < Time.time - 3f)){
			chasingPlayer = false;
			if(targetBeforeChasingPlayer != null){
				PathTo((int)targetBeforeChasingPlayer.Value.x,(int)targetBeforeChasingPlayer.Value.y,(int)targetBeforeChasingPlayer.Value.z);
			}else{
				RandomPathing();
			}
		}
	}
	
	public void RandomPathing(){
		//Debug.Log ("REpath");
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