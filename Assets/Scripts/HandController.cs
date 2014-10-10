//File: HandController.cs
//Project: Mastering Leap Motion
//Date: August 13, 2014
//
//Author: Brandon Sanders <brandon@mechakana.com>
//
///////////////////////////////////////////////////////////////////////////////
//Copyright (c) 2013 - 2014 Brandon Sanders <brandon@mechakana.com>
/*
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
///////////////////////////////////////////////////////////////////////////////
//
using UnityEngine;
using System.Collections;

//Class: HandController////////////////////////////////////////////////////////
//
//This class tracks the first hand in the Leap's field of view, generating
//controller-like inputs for it.
//
class HandController : BaseSingleton<HandController>
{
//Public///////////////////////////////////////////////////////////////////////
	
	//Leap Listener reference.
	public LeapListener listener;
	
	//Left joystick X/Y.  Corresponds to pitch and roll of the hand.
	public int x = 0;
	public int y = 0;
	
	//Should WASD input be prefered over LEAP input?
	public static bool wasdInput = false;
	
	//Is WASD input being forced due to a lack of LEAP input?
	public static bool wasdInputForced = false;
	
	//onAwake////////////////////////////////////////////////////////////////////
	public override void onAwake() { listener = new LeapListener(); }

	//Member Function: Update////////////////////////////////////////////////////
	public void Update() 
	{
		//Try to read data from the Leap Motion device.
		try
		{
			//Refresh the Leap data.
			listener.refresh();
			
			//Reset all of the variables.
			x = y = 0;
			
			//If there are hands in the field of view and the leap device is connected, use the LEAPs input.
			if (!wasdInput && LeapListener.connected && listener.hands > 0 && listener.fingers > 0)
			{
				//Do not force WASD input.
				wasdInputForced = false;
				
	        	//If there's a hand in the field of view (FOV), track it.
	        	if (listener.hands > 0 && listener.fingers > 0)
	        	{
	    			//Right joystick X-Axis.  Sensitivity is doubled since a value of '40' is more like '80'.
	    			x = (int) (listener.handRoll * -2);
	    			
	    			//Right joystick Y-Axis.  Sensitivity is doubled since a value of '40' is more like '80'.
	    			y = (int) (listener.handPitch * -2);
				}
			}
			
			//Do not force WASD input if the LEAP device is connected and WASD input is turned on.
			else if (wasdInput || LeapListener.connected)
				wasdInputForced = false;
			
			//Force WASD input.
			else
				wasdInputForced = true;
			
			//If a LEAP device is not connected, try the keyboard.  I'm sure the pros will prefer this.
			if (wasdInput || wasdInputForced)
			{
				//Left joystick X-Axis.
				if (Input.GetKey("a"))
					x += -80;
				if (Input.GetKey("d"))
					x += 80;
				
				//Left joystick Y-Axis.
				if (Input.GetKey("w"))
					y += 80;
				if (Input.GetKey("s"))
					y += -80;
			}
		}

		//If reading data fails, assert that WASD input is now being used.
		catch(System.Exception e){wasdInput = true;}
	}
}