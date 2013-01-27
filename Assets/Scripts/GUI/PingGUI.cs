using UnityEngine;
using System.Collections;

public class PingGUI : MonoBehaviour {
	static PingGUI instance;
	
	public PopupText popupText;
	
	void Awake()
	{
		if( instance == null )
			instance = this;
	}
	
	public static void PopupText( string text )
	{
		instance._PopupText( text );
	}
	
	void _PopupText( string text )
	{
		popupText.Go( text );
	}
}
