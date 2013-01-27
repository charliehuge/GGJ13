using UnityEngine;
using System.Collections;

public class AddStuffToMe : MonoBehaviour 
{
	public GameObject stuffToAdd;
	
	void Start()
	{
		GameObject go = GameObject.Instantiate(stuffToAdd) as GameObject;
		go.transform.parent = transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
	}
}
