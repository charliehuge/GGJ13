using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AlwaysSeekPlayer : PathingEntity {
	public EnemyColliderPropigator playerDetector;
	
	void Start () {
		playerDetector.CollisionPropigate =(bool foo)=>{}; //don't care
	}
	
	public override void OnReachPathEnd (int x, int y, int z)
	{
		PathTo(EntityController.GetInstance().playerEntity.x,EntityController.GetInstance().playerEntity.y,EntityController.GetInstance().playerEntity.z);
	}
	
	public override void OnInitializeEntity ()
	{
		base.OnInitializeEntity();
		playerDetector.Init();
	}
	
}