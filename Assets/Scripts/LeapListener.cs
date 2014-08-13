//File: LeapListener.cs
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
using Leap;

//Class: LeapListener//////////////////////////////////////////////////////////
//
//The LeapListener class opens a connection to any connected Leap Motion
//controller, and then reads in its data frame-by-frame.
//
//This class is just a slight re-write of the sample LeapListener class
//provided by the Leap Developers.
//
public class LeapListener
{    
//Private\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	
	//Leap controller.
	private Leap.Controller controller = null;
	
//Public///////////////////////////////////////////////////////////////////////
	
	//Minimum distance from hand for thumb to be recognized.
	public static float thumbDistance = 40;
	
	//Current frame.
	public Frame frame = null;
	
	//Fingers contained in the current frame.
	public int fingers = 0;
	
	//Hands contained in the current frame.
	public int hands = 0;
	
	//Various easy-access hand values.
	public float handPitch = 0.0F;
	public float handRoll = 0.0F;
	public float handYaw = 0.0F;
	
	//Hand and finger positions.
	public Vector handPosition = Vector.Zero;
	public Vector fingerPosition = Vector.Zero;
	public Vector handDirection = Vector.Zero;
	public Vector fingerDirection = Vector.Zero;
	
	//Quick-find for the right thumb.
	public Leap.Finger thumb = null;
	
	//Timestamp of the current frame.
	public long timestamp = 0;
	
	//Is the leap connected?
	public static bool connected = false;

	//Member Function: refresh/////////////////////////////////////////////////
	//
	//This function refreshes the tracking data from the Leap Motion device.
	//
	public bool refresh() 
	{
		//Try.
		try
		{
			//If there's no controller, make a new one.
			if (controller == null) controller = new Leap.Controller();
			
			//Check if the controller is connected.
			connected = controller.IsConnected && controller.Devices.Count > 0 && controller.Devices[0].IsValid;
			
			//If we're connected, update.
			if (connected)
			{
				//Get the most recent frame.
				frame = controller.Frame();
				
				//Assign some basic information from the frame to our variables.
				fingers = frame.Fingers.Count;
				hands = frame.Hands.Count;
				timestamp = frame.Timestamp;
				
				//If we see some hands, get their positions and their fingers.
				if (!frame.Hands.IsEmpty)
				{
					//Get the hand's position, size and first finger.
					handPosition = frame.Hands[0].PalmPosition;
					handDirection = frame.Hands[0].Direction;
					fingerPosition = frame.Hands[0].Fingers[0].TipPosition;
					fingerDirection = frame.Hands[0].Fingers[0].Direction;
					
					//Get the hand's normal vector and direction
					Vector normal = frame.Hands[0].PalmNormal;
					Vector direction = frame.Hands[0].Direction;
					
					//Get the hand's angles.
					handPitch = (float) direction.Pitch * 180.0f / (float)System.Math.PI;
					handRoll = (float) normal.Roll * 180.0f / (float)System.Math.PI;
					handYaw = (float) direction.Yaw * 180.0f / (float)System.Math.PI;
					
					thumb = null;
					
					//Find the thumb for the primary hand.
					foreach (Leap.Finger finger in frame.Hands[0].Fingers)
					{	
						if (thumb != null && finger.TipPosition.x < thumb.TipPosition.x && finger.TipPosition.x < handPosition.x)
							thumb = finger;
						
						else if (thumb == null && finger.TipPosition.x < handPosition.x - thumbDistance)
							thumb = finger;
					}
				}
			}
			
			//Otherwise, reset all outgoing data to 0.
			else
			{
				//Fingers contained in the current frame.
				fingers = 0;
				
				//Hands contained in the current frame.
				hands = 0;
				
				//Various easy-access hand values.
				handPitch = 0.0F;
				handRoll = 0.0F;
				handYaw = 0.0F;
				
				//Hand and finger positions.
				handPosition = Vector.Zero;
				fingerPosition = Vector.Zero;
				handDirection = Vector.Zero;
				fingerDirection = Vector.Zero;
				
				//Quick-find for the right thumb.
				thumb = null;
			}
			
			return true;
		}
		
		catch(System.Exception e) {/*Log.Write(e.StackTrace, this);*/  return false;}
	}
	
	//Mwmber Function: rotation////////////////////////////////////////////////
	//
	//Utility function for calculating the rotation of a hand.
	//
	public UnityEngine.Vector3 rotation (Leap.Hand hand)
	{
		//Create a new vector for our angles.
		UnityEngine.Vector3 rotationAngles = new UnityEngine.Vector3(0, 0, 0);
		
		//Get the hand's normal vector and direction
		Vector normal = hand.PalmNormal;
		Vector direction = hand.Direction;
		
		//Set the values.
		rotationAngles.x = (float) direction.Pitch * 180.0f / (float)System.Math.PI;
		rotationAngles.z = (float) normal.Roll * 180.0f / (float)System.Math.PI;
		rotationAngles.y = (float) direction.Yaw * 180.0f / (float)System.Math.PI;
		
		//Return the angles.
		return rotationAngles;
	}
}