using UnityEngine;
using System.Collections;

public class TestEnemy2 : OCDPatrollingEntity {

	public override void OnInitializeEntity ()
	{
		base.OnInitializeEntity();
	}
	
	public override void OnReachBlockCenter(int x, int y, int z)
	{
		base.OnReachBlockCenter(x,y,z);
	}

}
