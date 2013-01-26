using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;

public class PushableBlock : Entity 
{
	public override void OnInitializeEntity ()
	{
		base.OnInitializeEntity ();
		
		this.entityType = "PushableBlock";
	}
}
