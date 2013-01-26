/// <summary>
/// Entry point into all the scripts
/// </summary>
using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	public bool debug = false;
	
	public MapCurve mapCurve;
	
	public GUISkin hudSkin;
	public static GUISkin HUDSkin
	{
		get { return instance.hudSkin; }
	}
	
	Level currentLevel;
	int currentLevelIndex;
	
	static Game instance;
	
	void Awake()
	{
		if( instance != null )
		{
			Debug.LogError( "There can only be one game. Deleting.");
			GameObject.Destroy( gameObject );
			return;
		}
		
		instance = this;
	}
	
	void Start()
	{
		if( !debug )
			LoadLevel( mapCurve, 0 );	
	}
	
	public static void FinishCurrentLevel()
	{
		instance.LoadNextMap();
	}
	
	void LoadNextMap()
	{
		instance.LoadLevel( mapCurve, currentLevelIndex + 1 );	
	}
	
	void LoadLevel( MapCurve curve, int mapIndex )
	{
		if( currentLevel != null )
			currentLevel.Unload();
		
		currentLevel = Level.Load( mapCurve.Levels[ mapIndex ] );
		currentLevelIndex = mapIndex;
	}
}