/// <summary>
/// Entry point into all the scripts
/// </summary>
using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	public bool debug = false;
	
	public MapCurve mapCurve;
		
	public Color ambientLight = Color.black;
	
	Level currentLevel;
	int currentLevelIndex;
	
	static Game instance;
	
	void Awake()
	{
		Application.targetFrameRate = 60;
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
		RenderSettings.ambientLight = ambientLight;
		
		if( !debug )
			LoadLevel( mapCurve, 0 );
	}
	
	public static void FinishCurrentLevel()
	{
		instance.LoadNextMap();
	}
	public static void ReloadCurrentLevel()
	{
		instance.LoadLevel(instance.mapCurve,instance.currentLevelIndex);
	}
	
	void LoadNextMap()
	{
		if( !debug )
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