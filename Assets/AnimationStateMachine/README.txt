           __                                       
  ___ ___ /\_\   __  _    __      ___ ___     ___   
/' __` __`\/\ \ /\ \/'\ /'__`\  /' __` __`\  / __`\ 
/\ \/\ \/\ \ \ \\/>  <//\ \L\.\_/\ \/\ \/\ \/\ \L\ \
\ \_\ \_\ \_\ \_\/\_/\_\ \__/.\_\ \_\ \_\ \_\ \____/
 \/_/\/_/\/_/\/_/\//\/_/\/__/\/_/\/_/\/_/\/_/\/___/ 

===============================================================
Follow the steps listed below to set up your Motion Pack In Unity3d.
===============================================================

1. Create an Empty GameObject that will act as the master node for the character or NPC

2. Place the character (mesh with skeleton and animations applied) into the new Empty GameObject. (Thus making it a child of the Empty GameObject). Be sure to zero out the position so it is centered in the empty game object.

3. Add the AnimationStateMachine.cs script to the Empty GameObject. (Located in the Scripts folder)

4. Set the Target (field of AnimationStateMachine) to be the character placed inside the Empty GameObject.

5. Set the Graph Text Asset (field of AnimationStateMachine) to be the JSON .txt (demo_motion_pack.txt) file containing all the state information. (Located in the Scripts folder)

6. Set the Root Motion Mode (field of AnimationStateMachine) to "Manual".

7. Add a CharacterController to the Empty GameObject.

8. Set it up to line up with the character's feet.

9. Add the provided controller (MixamoDemoControlScript.cs) or your own custom controller. (Located in the Scripts folder)

10. Play!

==============================================================
==============================================================

NOTE: 

A blank graph and blank controller have been provided in the Blank Scripts for You folder. They are a good place to start if your planning on using the RMCv2:ASM with your own characters and games

==============================================================
==============================================================

WANT A VIDEO WALKTHROUGH?

Watch all our video tutorials here:

http://www.mixamo.com/c/tutorials#motionpacks

==============================================================

Still Stuck?!

Need Help?!   
    
E-mail Us!

Support@Mixamo.com
