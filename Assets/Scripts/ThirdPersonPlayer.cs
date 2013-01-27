using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonPlayer : Entity 
{
	public float grabDistance = 1f;
	
	public List<Light> switchableLights;
	
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
		
		cameraRoot = new GameObject("Camera Root");
		
		cameraRoot.transform.parent = transform;
		cameraRoot.transform.localPosition = Vector3.zero;
		cameraRoot.transform.forward = transform.forward;
		cameraRoot.transform.parent = null;
		
		
		playerCamera.transform.parent = cameraRoot.transform;
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
		playerCamera.transform.localPosition = new Vector3( 0, camHeight, -camDistance );
		
		playerCamera.transform.LookAt( camLookAt );
	}
	
	public override void OnUpdateEntity (float deltaTime)
	{
		UpdateGravity();
		HandleInput();
		UpdateMovement(Time.deltaTime);
		
		//We'll just assure our states are all nice
		bool moving = inputVector != Vector2.zero;
		SetIsIdle( !moving );
		SetIsMoving( moving );
	}
	
	//Update the falling flags for the character
	void UpdateGravity(){
		isFalling = (cc.collisionFlags & CollisionFlags.Below) == 0;
	}
	
	void HandleInput(){
		inputVector.x = Input.GetAxis("Horizontal");
		inputVector.y = Input.GetAxis ("Vertical");
		
		if( Input.GetButtonDown("Jump") )
		{
			foreach( Light l in switchableLights )
			{
				l.enabled = !l.enabled;	
			}
		}
	}
		
	void UpdateMovement(float deltaTime)
	{	
		Vector3 movement = Vector3.zero;
					
		if(isFalling) movement.y -= gravity;
		
		movement.x += walkSpeed * inputVector.x;
		movement.z += walkSpeed * inputVector.y;
		
		Vector3 facingDir = Vector3.Normalize( new Vector3( movement.x, 0f, movement.z ) );
		if( facingDir != Vector3.zero )
		{
			cc.transform.forward = facingDir;	
		}
		
		movement *= deltaTime;
		
		cc.Move(movement);			
	}
}
