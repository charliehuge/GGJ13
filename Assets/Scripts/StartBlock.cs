using UnityEngine;
using System.Collections;

public class StartBlock : MonoBehaviour 
{
	public ThirdPersonPlayer playerPrefab;
	
	ThirdPersonPlayer player;
	
	void Start()
	{
		if( player == null )
		{
			player = GameObject.Instantiate( playerPrefab ) as ThirdPersonPlayer;
			Block b = transform.parent.gameObject.GetComponent<Block>();
			player.transform.parent = b.blockMap.transform;
			player.transform.position = transform.position;
			player.InitializeEntity( b.blockMap );
		}
	}
}
