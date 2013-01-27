using UnityEngine;
using System.Collections;

public class EndCredits : MonoBehaviour 
{
	public UILabel theText;
	
	public float rollTime = 20f;
	Vector3 startPos;
	public Transform endXform;
	Vector3 endPos;
	
	string credits = "PING\n\n" +
		"Developed by Team PING at #GGJ13 in SF:\n\n" +
		"Alex (@alexsink)\n" +
		"Archie (@cyborgdino)\n" +
		"Charlie (@charliehuge)\n" +
		"Whit\n" +
		"\n" + 
		"Music: \"Tranquility\" by Kevin MacLeod (incompetech.com)\n";
	
	void Awake()
	{
		theText.text = "";
		startPos = theText.transform.localPosition;
		endPos = endXform.localPosition;
	}
	
	public void Go()
	{
		theText.text = credits;
		StartCoroutine( RollEm() );
	}
	
	IEnumerator RollEm()
	{
		float elapsed = 0f;
		
		while( elapsed < rollTime )
		{
			elapsed += Time.deltaTime;
			float lerpTime = elapsed / rollTime;
			theText.alpha = lerpTime;
			theText.transform.localPosition = Vector3.Lerp( startPos, endPos, lerpTime );
			yield return null;
		}
	}
}
