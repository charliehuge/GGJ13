using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonPlayer : Entity 
{
	public float baseHearingRadius = 2f;
	public float stillHearingRadius = 2f;
	public float lightOffHearingRadius = 2f;
	
	bool lightIsOn = true;
	
	[HideInInspector]
	public float totalHearingRadius;
	
	public List<GameObject> switchableLights;
	
	//The speed at which the character walks
	public float walkSpeed = 2f;
	//The speed at which the character falls
	public float gravity = 9.8f;
	//The CharacterController that the character will utilise for movement
	protected CharacterController cc;
		
	//Our input states
	protected bool isFalling = false;
	Vector2 inputVector;
	
	PushableBlock focusedBlock;
	PushableBlock grabbedBlock;

	#region Camera
	
	//See the InitializeCamera() function to discover the function of this
	public bool bindCameraToPlayer = true;
	protected Camera playerCamera;
	protected GameObject cameraRoot;
	
	public Transform camLookAt;
	public float heightBuffer = 0.1f;
	public float cameraSpeedIn = 1.0f;
	public float cameraSpeedOut = 1.0f;
	
	public AnimationCurve camDistanceCurve;
	public AnimationCurve camHeightCurve;
	
	private float currentCamLerp = 0.2f;
	
	private float playerIdleTime = 0f;
	
	#endregion
	
	protected bool initialized = false;
		
	public override void OnInitializeEntity ()
	{	
		if(initialized){
			return;
		}
		
		initialized = true;
		
		//Retrieve our character controller
		this.cc = gameObject.GetComponent<CharacterController>();
		
		controller.RegisterPlayer(this);
		
		//Place the camera so we can see everything nicely
		InitializeCamera();		
	}
	
	void InitializeCamera(){

		
		//What we're going to do with our camera is:
		//Create a new gameobject that will follow the character
		//Bind the camera to this object, at an offset
		//This way, the character can rotate and such
		//But the camera will behave as if it's parented to it
				
		playerCamera = Camera.main;
		
		if(playerCamera == null){
			playerCamera = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
		}
		
		if(playerCamera == null){
			GameObject o = new GameObject("Player_Camera");
			playerCamera = o.AddComponent<Camera>();
		}
		
		
			if(cameraRoot == null || cameraRoot == transform){
		cameraRoot = new GameObject("Camera Root");
		
		cameraRoot.transform.parent = transform;
		cameraRoot.transform.localPosition = Vector3.zero;
		cameraRoot.transform.forward = transform.forward;
		cameraRoot.transform.parent = null;
		
		
		playerCamera.transform.parent = cameraRoot.transform;
		}
	}
	
	void LateUpdate(){
		UpdateCamera();
	}
	
	void UpdateCamera(){
		
		if(playerCamera == null){
			return;
		}
		
		cameraRoot.transform.position = transform.position;
		
		if( isMoving )
		{
			playerIdleTime = 0f;
			currentCamLerp -= Time.deltaTime * cameraSpeedIn;
		}
		else
		{
			playerIdleTime += Time.deltaTime;
			if( playerIdleTime >= 1f )
				currentCamLerp += Time.deltaTime * cameraSpeedOut;
		}
		
		currentCamLerp = Mathf.Clamp( currentCamLerp, 0f, 1f );
		
		float camDistance, camHeight;
		camDistance = camDistanceCurve.Evaluate( currentCamLerp );
		camHeight = camHeightCurve.Evaluate( currentCamLerp );
		playerCamera.transform.localPosition = new Vector3( 0, camHeight+heightBuffer, -camDistance );
		
		playerCamera.transform.LookAt( camLookAt );
		playerCamera.transform.Rotate(Vector3.up * 10);
		//playerCamera.transform.Rotate(Vector3.left * 10);
	}
	
	public override void OnUpdateEntity (float deltaTime)
	{
		UpdateGravity();
		HandleInput();
		
		//We'll just assure our states are all nice
		bool moving = !Mathf.Approximately( inputVector.y, 0f );
		SetIsIdle( !moving );
		SetIsMoving( moving );
		
		UpdateHearingRadius();
	}
	
	void UpdateHearingRadius()
	{
		totalHearingRadius = baseHearingRadius;
		if( !lightIsOn )
			totalHearingRadius += lightOffHearingRadius;
		if( !isMoving )
			totalHearingRadius += stillHearingRadius;
	}
	
	//Update the falling flags for the character
	void UpdateGravity(){
		isFalling = (cc.collisionFlags & CollisionFlags.Below) == 0;
		
		if( isFalling )
			cc.Move( new Vector3( 0f, -1f * gravity * Time.deltaTime, 0f ) );
	}
	
	void HandleInput(){
		inputVector.x = Input.GetAxis("Horizontal");
		inputVector.y = Input.GetAxis ("Vertical");
		
		if( Input.GetButtonDown("Jump") )
		{
			lightIsOn = !switchableLights[0].activeSelf;
			
			foreach( GameObject l in switchableLights )
			{
				l.SetActive( lightIsOn );	
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere( transform.position, totalHearingRadius );
	}
}
