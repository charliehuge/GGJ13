using UnityEngine;
using System.Collections;

public class TestEnemyScript : OCDPatrollingEntity {
	public EnemyColliderPropigator playerDetector;
	bool detectedPlayerThisFrame = false; 
	float lastTimeDetectedPlayer = 0f;
	bool playerInSensor = false;
	bool chasingPlayer = false;  //we likely want to control how long we "remember" where to chase
	Vector3 lastPlayerMapIndex;  //should be int3
	Vector3 regularPatrolPathEnd;
		
	int numberOfStepsTillIForgetWhenPlayer = 1;
	
	// Use this for initialization
	void Start () {
		playerDetector.CollisionPropigate =(bool foo)=>{};
		//playerDetector.CollisionPropigate = OnDetectedPlayer;
		//findOpenBlockAdjacentToPlayer();
	}
	
	/*public override void OnInitializeEntity ()
	{
		//base.OnInitializeEntity();
		
	}*/
	
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
#if false
		lastTimeDetectedPlayer = Time.time;
		playerInSensor = foundPlayer;
		chasingPlayer = true;
		
		lastPlayerMapIndex = new Vector3(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
		Debug.Log ("Player index: " + lastPlayerMapIndex);
		//DoPlayerChase();
		
		if(foundPlayer){
			regularPatrolPathEnd = targetPosition;
			
			Debug.LogError ("Found player");
			//seek out player for ... pinging
			detectedPlayerThisFrame = true;
			Debug.Log ("block next to player: " + findOpenBlockAdjacentToPlayer());
		}else{
			Debug.LogError ("lost player");
			detectedPlayerThisFrame = false;
			//we lost player, wander around again
		}
#endif
	}
	
#if false
	public override void OnReachBlockCenter (int x, int y, int z)
	{
		//base.ProcessPatrolNode();
		
		bool chaseNow = false;
		if(chasingPlayer){
			if(playerInSensor == false && lastTimeDetectedPlayer < Time.time - 3f){
				chasingPlayer = false; //stop chasing them if we haven't seen them in 3 seconds
				//ResumePatrol();
			}else{
				lastPlayerMapIndex = new Vector3(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
				chaseNow = true;
			}
		}
		
		if(false){
			Debug.LogError("Chasing player");
			DoPlayerChase();
			base.ProcessPatrolNode();
		}else{
			base.ProcessPatrolNode();
			//base.ProcessPatrolNode();  // I think this should resume
			//ProcessPatrolNode();
		}
	}
#endif
}
