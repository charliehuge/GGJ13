using UnityEngine;
using System.Collections;

public class PopupText : MonoBehaviour 
{
	public UILabel label;
	public float fadeTime = 1f;
	
	Vector3 startPos;
	Vector3 endPos;
	public Transform endTransform;
	
	void Awake()
	{
		label.text = "";
		startPos = label.transform.localPosition;
		endPos = endTransform.localPosition;
	}
	
	public void Go( string text, float closeInSeconds = 2f )
	{
		label.text = text;
		StartCoroutine( Animate( closeInSeconds ) );
	}
	
	IEnumerator Animate( float waitTime )
	{
		float t = 0f;
		while( t < fadeTime )
		{
			float lerpAmount = t / fadeTime;
			label.alpha = lerpAmount;
			label.transform.localPosition = Vector3.Lerp( startPos, endPos, lerpAmount );
			t += Time.deltaTime;
			yield return null;
		}
		
		yield return new WaitForSeconds( waitTime );
		
		t = 1f;
		while( t > 0f )
		{
			label.alpha = t / fadeTime;
			t -= Time.deltaTime;
			yield return null;
		}
	}
}
