using UnityEngine;
using System.Collections;

public class GoalBlock : TidyMapBoundObject
{
	public Pulse pulsePrefab;
	
	Pulse pulse;
	
	void Awake()
	{
		if( pulse == null )
		{
			pulse = GameObject.Instantiate( pulsePrefab ) as Pulse;
			pulse.transform.parent = transform;
			pulse.transform.localPosition = Vector3.zero;
			pulse.transform.localRotation = Quaternion.identity;
		}
	}
		
	#region implemented abstract members of TidyMapBoundObject
	public override void OnObjectEnterBlock (Block b, TidyMapBoundObject e)
	{
		if( e is ThirdPersonPlayer )
		{
			Game.FinishCurrentLevel();
		}
	}

	public override void OnObjectExitBlock (Block b, TidyMapBoundObject e)
	{
		// empty
	}
	#endregion
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere( transform.position, .5f );
	}
}
