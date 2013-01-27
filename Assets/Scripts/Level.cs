using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelDefinition
{
	public string Name = "New Level";
	public string IntroText = "Some intro text";
	public bool IsEndCredits = false;
	public BlockMap MapPrefab;
}

public class Level
{
	LevelDefinition definition;
	BlockMap mapInstance;
	
	public static Level Load( LevelDefinition def )
	{
		Level l = new Level();
		l.definition = def;
		l.Begin();
		return l;
	}
	
	public void Unload()
	{
		if( mapInstance != null )
		{
			GameObject.Destroy( mapInstance.gameObject );
			mapInstance = null;
		}
	}
	
	public string GetName()
	{
		return definition.Name;	
	}
	
	void Begin()
	{
		if( definition.IsEndCredits )
		{
			PingGUI.RollCredits();
		}
		else
		{
			// do intro stuff here
			PingGUI.PopupText( definition.IntroText );
			
			LoadMap();
		}
	}
	
	void LoadMap()
	{
		mapInstance = GameObject.Instantiate( definition.MapPrefab ) as BlockMap;
	}
}