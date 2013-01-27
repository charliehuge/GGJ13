using UnityEngine;
using System.Collections;

public class TestEnemyScript : OCDPatrollingEntity {
	public EnemyColliderPropigator playerDetector;
	float lastTimeDetectedPlayer = 0f;
	bool playerInSensor = false;
	bool chasingPlayer = false;  //we likely want to control how long we "remember" where to chase
	Vector3 lastPlayerMapIndex;  //should be int3
	Vector3 regularPatrolPathEnd;
		
	int numberOfStepsTillIForgetWhenPlayer = 1;
	
	// Use this for initialization
	void Start () {
		//playerDetector.CollisionPropigate =(bool foo)=>{};
		playerDetector.CollisionPropigate = OnDetectedPlayer;
		//findOpenBlockAdjacentToPlayer();
	}
	
	public override void OnInitializeEntity ()
	{
		base.OnInitializeEntity();
		playerDetector.Init();
		
		/*if(playerDetector.collider.bounds.Contains(playerDetector.playerObj.transform.position)){
			OnDetectedPlayer(true);
		}*/
	}
	
	bool CanMoveTo(Vector3 pos)
	{
		return CanMoveTo((int)pos.x,(int)pos.y,(int)pos.z);
	}
	
	Vector3 findOpenBlockAdjacentToPlayer ()
	{
		Vector3? targetBlock = null;
		Vector3 modifier = Vector3.zero;
		
		for(int i=-1;i<2;i++){
			for(int j=-1;j<2;j++){
				Vector3 posTrying = lastPlayerMapIndex + new Vector3(i,j,0);
				posTrying.z = this.z; //we can only move on our z
				
				if(CanMoveTo(posTrying)){
					targetBlock = posTrying;
					break;
				}
			}
			if(targetBlock != null){
				break;
			}
		}
		
		if(targetBlock == null){
			Debug.LogWarning("couldn't path to player");
			return Vector3.zero;
		}else{
			return targetBlock.Value;
		}
	}
	
	void DoPlayerChase()
	{
		Vector3 taretMapPos = findOpenBlockAdjacentToPlayer();
		
		
		if(taretMapPos == Vector3.zero){
			//wait a processing step
			ResumePatrol();
			return;
		}
		
		targetPosition = taretMapPos;
		
	}
	
	void ResumePatrol()
	{
		targetPosition = regularPatrolPathEnd;
		
	}
	
	
	void OnDetectedPlayer(bool foundPlayer)
	{
		lastTimeDetectedPlayer = Time.time;
		playerInSensor = foundPlayer;
		chasingPlayer = true;
		
		lastPlayerMapIndex = new Vector3(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
		Debug.Log ("Player index: " + lastPlayerMapIndex);
		//DoPlayerChase();
		
		if(foundPlayer){
			regularPatrolPathEnd = targetPosition; //TODO: make sure this is always from 
			
			Debug.LogError ("Found player");
			//seek out player for ... pinging
			Debug.Log ("block next to player: " + findOpenBlockAdjacentToPlayer());
		}else{
			Debug.LogError ("lost player");
			//we lost player, wander around again
		}
	}
	
	public override void OnReachBlockCenter (int x, int y, int z)
	{
		//base.ProcessPatrolNode();
		
		bool chaseNow = false;
		if(chasingPlayer){
			if(playerInSensor == false /* && lastTimeDetectedPlayer < Time.time - 3f */){
				chasingPlayer = false; //stop chasing them if we haven't seen them in 3 seconds
				ResumePatrol();  //swap back the regular patrol path end
			}else{
				lastPlayerMapIndex = new Vector3(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
				chaseNow = true;
			}
		}
		//Debug.Log ("processign: target node: " + targetPosition);
		if(chasingPlayer){
			Debug.LogError("Chasing player");
			DoPlayerChase();
			//base.ProcessPatrolNode();
		}else{
			Debug.LogError("Not chasing player");
			base.ProcessPatrolNode();
			//base.ProcessPatrolNode();  // I think this should resume
			//ProcessPatrolNode();
		}
	}
	
	public override void ProcessPatrolNode(){
		
		sourcePosition = eTransform.localPosition;
				
		if(IsStuck()){
			OnCannotMove(x,y,z);
			return;
		}
		
		if(patrolAxis == PatrolAxis.Horizontal){
			targetCoords = new Vector3(x + currentDirection,y,z);
			targetPosition = BlockUtilities.GetMathematicalPosition(parentMap,x + currentDirection,y,z);
						
		}
		else if(patrolAxis == PatrolAxis.Vertical){
			targetCoords = new Vector3(x,y+currentDirection,z);
			targetPosition = BlockUtilities.GetMathematicalPosition(parentMap,x,y+currentDirection,z);
			
		}
		
		if(!CanMoveTo((int)targetCoords.x,(int)targetCoords.y,(int)targetCoords.z)){
						
			ReachEndOfPatrol(x,y,z);
			
			return;
		}
		controller.ReserveBlock(entityType,targetCoords);
		
		//Now decide where we're facing
		int x_dir = x - (int)targetCoords.x;
		int y_dir = (int)targetCoords.y - y;
		
		if(x_dir > 0){
			eTransform.up = upDirection;
			eTransform.forward = rightDirection;
		}
		else if(x_dir < 0){
			eTransform.up = upDirection;
			eTransform.forward = leftDirection;
		}
		else if(y_dir > 0){
			
			if(parentMap.growthAxis == BlockMap.GrowthAxis.Forward){
				eTransform.forward = frontDirection;	
			}
			else{
				eTransform.forward = downDirection;
				Vector3 rot = eTransform.localRotation.eulerAngles;
				rot.y = 0.0f;
				eTransform.localRotation = Quaternion.Euler(rot);
			}
			
		}
		else if(y_dir < 0){
			
			if(parentMap.growthAxis == BlockMap.GrowthAxis.Forward){
				eTransform.forward = backDirection;	
			}
			else{
				eTransform.forward = upDirection;
			}
			
		}
		
		SetIsMoving(true);
		
	}
}
