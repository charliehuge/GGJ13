using UnityEngine;
using System.Collections;

public class GoalBlock : TidyMapBoundObject
{
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
}
