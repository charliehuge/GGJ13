using UnityEngine;
using UnityEditor;
using System;

public class CreateMapCurve 
{
	[MenuItem("Assets/Create/Map Curve")]
	public static void Create()
	{
		CustomAssetUtility.CreateAsset<MapCurve>();
	}
}
