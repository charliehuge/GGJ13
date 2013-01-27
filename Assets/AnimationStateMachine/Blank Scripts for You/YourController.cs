using UnityEngine;
using System.Collections;

public class YourController : MonoBehaviour, Mixamo.TransitionHandler {
//=====================================================================================================
//You can add to this section but || DO NOT REMOVE ANYTHING CURRENTLY PRESENT || unless you
//know what your doing and comfortable with how this and the other script work together.


	// Template Guard
	public bool CanTransitionTo (string guard, string source, string destination)
	{
		return true;
	}

	public string[] KeyControls() {
		return keys;
	}
	
	// Find Animation State Machine on gameObject
	AnimationStateMachine GetASM() {
		return this.GetComponent<AnimationStateMachine>();
	}
	
	// Let Transition Handler know that this is the object it should be watching for transition information
	void Start () {
		GetASM().SetTransitionHandler( this );
		controller = GetComponent<CharacterController>();
		asm = GetASM();
	}
//=====================================================================================================

	//  Variables for users
	public bool ShowGUIKey = true;
	public float turnDegrees = 45f;
	public float jumpSpeed = 4.0f;
	
	//private vars for internal systems such a idling and gravity
	private int turnDirection = 0;
	private Vector3 moveDirection = Vector3.zero;

	
	// Variables for controllers and global scripts
	private AnimationStateMachine asm;
	private AnimationStateMachine.RootMotionResult result;
	private CharacterController controller;
	private string[] keys = {
		"W", "Forward", 
		"A", "Turn Left", 
		"D", "Turn Right",
		"Shift", "Run"
	};
	
	// if GUI Keys is on then show the key controls in the upper left hand corner.
	void OnGUI() {
		if( ShowGUIKey ) {
			GUILayout.BeginVertical( GUILayout.ExpandWidth( true ) );
			GUILayout.Label( "Key Options:" );
			
			// loop keys to screen
			for (int i = 0; i < keys.Length; i += 2) {
				GUILayout.Label(keys[i] + " - " + keys[i+1]);
			}
			
			GUILayout.EndVertical();
		}
	}
	
	// Update is called once per frame and all transition (asm.ChangeState) conditions should take place in here.
	void Update () {
		
			
			// Movement based on key press + shift for running
			// This method ramps through two animation on a blend state in this case "move"
			// When the blend is = 1 the animation playing is walk
			// when the blend is = 0 the animation playing is run
			// Here we use the Mixamo.Util.CrossFadeDown/Up function that will fade down/up a value over the time specified
			// In this case we are using it to blend between walk and run and since all animation in a blend are synced this
			// prevents foot sliding while the changing from walk to run and vice versa.
			if( Input.GetKey( KeyCode.W ) ) {
				
				if( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) ) {
					
						asm.ControlWeights["ctrl_move"] = Mixamo.Util.CrossFadeDown( asm.ControlWeights["ctrl_move"] , 0.3f );
						
					} else {
						
						asm.ControlWeights["ctrl_move"] = Mixamo.Util.CrossFadeUp( asm.ControlWeights["ctrl_move"] , 0.3f );
						
					}
				asm.ChangeState( "move" );
					
			}
			
			// If no input we enter the idle animation.
			else {
				asm.ChangeState( "idle" );
			}
			
			// Turning Keys, Animations do no effect root motion. These turns are done procedurally 
			// to increases responsiveness while turning and stopping for the player
			if( Input.GetKey( KeyCode.A )) {
					turnDirection = -1; //left
			} 
			
			else if( Input.GetKey( KeyCode.D )) {
					turnDirection = 1; //right
			} 
			
			else {
				turnDirection = 0;
			}
			
			// This is the turning control. When the turn direction is above or below 0 we set the forward vector to the forward vector of the controller
			// we then normalize the forward vector from 0 allowing us to decare the right and left vectors as between 0-1, -1-0
			// finally in the last line we use Quaternion.LookRotaion to use the values of forward and right in Vector3.RotateTowards so we can use 
			// positive and negative 1(one) to drive the rotation based on the turnDegrees value. In other words
			// The value of turnDegrees is the amount of degrees the character will turn in 1(one) second.
			if( turnDirection != 0f ){
				Vector3 forward = this.transform.forward;
				forward.y = 0;
				forward = forward.normalized;
				Vector3 right = new Vector3(forward.z, 0, -forward.x);
				transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards( forward , right * turnDirection , turnDegrees * Mathf.Deg2Rad * Time.deltaTime , 1000f ) );
			}
			
			moveDirection.y = 0;
			
		}
		
	
	
	// Updates every frame but after Update() and should be used for things that happen based on the effects caused by actions in the Update()
	void LateUpdate() {
		if (controller != null){
		//AnimationStateMachine asm = GetASM();
			result = asm.GetRootMotion();
			if( result != null ) {
				// apply gravity
				Vector3 gravity = (Vector3.up * -9.8f * Time.deltaTime );
				controller.Move((moveDirection * Time.deltaTime) + gravity + result.GlobalTranslation);
			}
		}
	}
}

