using UnityEngine;
using System.Collections;

public class GoalBlock : TidyMapBoundObject
{
	// there can only be one...for now
	static GoalBlock instance;
	public static GoalBlock Instance { get { return instance; } }
	
	#region implemented abstract members of TidyMapBoundObject
	public override void OnObjectEnterBlock (Block b, TidyMapBoundObject e)
	{
		/*
		if( e is ThirdPersonPlayer )
		{
			Game.FinishCurrentLevel();
		}
		*/
	}

	public override void OnObjectExitBlock (Block b, TidyMapBoundObject e)
	{
		// empty
	}
	#endregion
}
