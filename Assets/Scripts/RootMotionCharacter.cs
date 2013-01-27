using UnityEngine;
using System.Collections;

public enum MovementMode
{
	Human,
	Zombie
}

[AddComponentMenu("Mixamo/Demo/Root Motion Character")]
public class RootMotionCharacter : MonoBehaviour
{
	public float turningSpeed = 90f;
	public RootMotionComputer computer;
	public CharacterController character;
	public MovementMode mode = MovementMode.Human;
	
	void Start()
	{
		// validate component references
		if (computer == null) computer = GetComponent(typeof(RootMotionComputer)) as RootMotionComputer;
		if (character == null) character = GetComponent(typeof(CharacterController)) as CharacterController;
		
		// tell the computer to just output values but not apply motion
		computer.applyMotion = false;
		// tell the computer that this script will manage its execution
		computer.isManagedExternally = true;
		// since we are using a character controller, we only want the z translation output
		computer.computationMode = RootMotionComputationMode.ZTranslation;
		// initialize the computer
		computer.Initialize();
		
		// set up properties for the animations
		animation["idle_2"].layer = 0; animation["idle_2"].wrapMode = WrapMode.Loop;
		animation["walking"].layer = 1; animation["walking"].wrapMode = WrapMode.Loop;
		animation["running"].layer = 1; animation["running"].wrapMode = WrapMode.Loop;
		//animation["zombiewalk"].layer = 2; animation["zombiewalk"].wrapMode = WrapMode.Loop;
		
		animation.Play("idle_2");
	}
	
	void Update()
	{
		float targetMovementWeight = 0f;
		float throttle = 0f;
		
		// turning keys
		if (Input.GetKey(KeyCode.A)) transform.Rotate(Vector3.down, turningSpeed*Time.deltaTime);
		if (Input.GetKey(KeyCode.D)) transform.Rotate(Vector3.up, turningSpeed*Time.deltaTime);
		
		// forward movement keys
		// ensure that the locomotion animations always blend from idle to moving at the beginning of their cycles
		if (Input.GetKeyDown(KeyCode.W) && 
			(animation["walking"].weight == 0f || animation["running"].weight == 0f))
		{
			animation["walking"].normalizedTime = 0f;
			animation["running"].normalizedTime = 0f;
		}
		if (Input.GetKey(KeyCode.W))
		{
			targetMovementWeight = 1f;
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) throttle = 1f;
				
		// blend in the movement
		if (mode == MovementMode.Human)
		{
			animation.Blend("running", targetMovementWeight*throttle, 0.5f);
			animation.Blend("walking", targetMovementWeight*(1f-throttle), 0.5f);
			// synchronize timing of the footsteps
			animation.SyncLayer(1);
		}
		else
		{    
			// ensure that the locomotion animations always blend from idle to moving at the beginning of their cycles
		//	if (Input.GetKeyDown(KeyCode.W) && 
		//	(animation["zombiewalk"].weight == 0f))
		//{
		//	animation["zombiewalk"].normalizedTime = 0f;
		//}

		//	animation.Blend("zombiewalk", targetMovementWeight, 0.5f);
		}
		
	}
	
	void LateUpdate()
	{
		computer.ComputeRootMotion();
		
		// move the character using the computer's output
		character.SimpleMove(transform.TransformDirection(computer.deltaPosition)/Time.deltaTime);
	}
}