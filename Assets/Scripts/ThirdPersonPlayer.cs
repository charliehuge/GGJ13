using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonPlayer : Entity {
	public float grabDistance = 1f;
	
	public Light flashlight;
	
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
	public float cameraDistance = 5.0f;
	public float cameraHeightOffset = 1.0f;
	
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
		playerCamera.transform.localPosition = new Vector3(0.0f,cameraHeightOffset,-cameraDistance);
		playerCamera.transform.LookAt( transform );
	}
	
	void LateUpdate(){
		UpdateCamera();
	}
	
	void UpdateCamera(){
		
		if(playerCamera == null){
			return;
		}
		
		cameraRoot.transform.position = transform.position;
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
		isFalling = cc != null && (cc.collisionFlags & CollisionFlags.Below) == 0;
	}
	
	void HandleInput(){
		inputVector.x = Input.GetAxis("Horizontal");
		inputVector.y = Input.GetAxis ("Vertical");
		
		if( Input.GetButtonDown("Jump") )
		{
			flashlight.enabled = !flashlight.enabled;	
		}
	}
		
	void UpdateMovement(float deltaTime)
	{	
		Vector3 movement = Vector3.zero;
					
		if(isFalling) movement.y -= gravity;
		
		movement.x += walkSpeed * inputVector.x;
		movement.z += walkSpeed * inputVector.y;
		
		Vector3 facingDir = Vector3.Normalize( new Vector3( movement.x, 0f, movement.z ) );
		if( grabbedBlock == null && facingDir != Vector3.zero )
		{
			cc.transform.forward = facingDir;	
		}
		
		movement *= deltaTime;
		
		// deal with blocks or normal movement
		if( grabbedBlock != null )
		{
			if(Input.GetButton("Fire1"))
			{
				DoPushMove( movement );
				return;
			}
			else
				DropBlock();
		}
		else if( CheckForPushableBlock() )
		{
			if( Input.GetButton("Fire1") )
			{
				GrabBlock();
				return;
			}
		}
		if(cc == null) return;
		cc.Move(movement);			
	}
	
	void GrabBlock()
	{
		grabbedBlock = focusedBlock;
		grabbedBlock.transform.parent = transform;
	}
	
	void DropBlock()
	{
		grabbedBlock.transform.parent = grabbedBlock.parentMap.transform;
		grabbedBlock = null;
	}
	
	void DoPushMove( Vector3 movement )
	{
		// constrain to moving on the axis of the block
		Vector3 dirToBlock = transform.position - grabbedBlock.transform.position;
		
		Vector3 moveScale;
		if( Mathf.Abs( dirToBlock.x ) > Mathf.Abs( dirToBlock.z ) )
			moveScale = new Vector3( 1, 0, 0 );
		else
			moveScale = new Vector3( 0, 0, 1 );
		
		Vector3 actualMove = Vector3.Scale( movement, moveScale );
		cc.Move( actualMove );
	}
			
	public bool CheckForPushableBlock()
	{
		Ray facingRay = new Ray( transform.position, transform.forward );
		
		RaycastHit hitInfo;
		if( Physics.Raycast( facingRay, out hitInfo, grabDistance, 1 << LayerMask.NameToLayer("PushableBlocks") ) )
		{
			GameObject thing = hitInfo.collider.gameObject;
			Debug.Log(thing.name);
			PushableBlock pb = thing.GetComponent<PushableBlock>();
			if( pb != null )
			{
				focusedBlock = pb;
				return true;
			}
		}
		
		focusedBlock = null;
		return false;
	}
	
	void OnGUI()
	{
		if( focusedBlock != null && grabbedBlock == null )
		{
			GUI.Label( new Rect( Screen.width / 2 - 150f, 20f, 300f, 100f ), 
				"Press Fire to grab", 
				Game.HUDSkin.label );	
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine( transform.position, transform.position + transform.forward * grabDistance );	
	}
}
