using UnityEngine;
using System.Collections;

public class TextTrigger : MonoBehaviour 
{
	public string text;
	public bool oneTime = true;
	
	bool triggered = false;
	
	void OnTriggerEnter( Collider c )
	{
		if( triggered ) return;
		
		ThirdPersonPlayer player = c.gameObject.GetComponent<ThirdPersonPlayer>();
		
		if( player != null )
		{
			PingGUI.PopupText( text );
			triggered = true;
		}
	}
}
