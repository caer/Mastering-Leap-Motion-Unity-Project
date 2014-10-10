//File: TouchPointer.cs
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
using System.Collections.Generic;

//Class: TouchPointer//////////////////////////////////////////////////////////
//
//This class generates virtual 2D pointers for every single
//finger tip within the Leap's field of view.  These pointers are automatically
//rendered onto Unity's GUI layer when the game is running.
//
class TouchPointer : BaseSingleton<TouchPointer>
{
//Private//////////////////////////////////////////////////////////////////////
	
	//Leap listener.
	private LeapListener listener;
	
	//Leap box.
	private Leap.InteractionBox normalizedBox;
	
//Public///////////////////////////////////////////////////////////////////////

	//Pointer texture.
	public Texture2D pointerNormal;
	
	//Vertical (Y-axis) offset of pointer coordinates.
	public float verticalOffset = -10.0F;
	
	//Currently active fingers.
	public List<Rect> fingers = new List<Rect>();

	//Member Function: onAwake///////////////////////////////////////////////////
	public override void onAwake() { listener = new LeapListener(); }

	//Member Function: OnDisable/////////////////////////////////////////////////
	public void OnDisable()
	{
		//Reset the fingers array.
		if (fingers != null)
			fingers.Clear();
	}
	
	//Member Function: Update////////////////////////////////////////////////////
	public void Update()
	{		
		//Update the listener.
		listener.refresh();

		//Reset the fingers array.
		if (fingers != null)
			fingers.Clear();

		//Retrieve coordinates for any fingers that are present, but only if the menus are visible.
		if (listener.fingers > 0 && Core.getInstance().paused)
		{
			//Loop over all fingers.
			for (int i = 0; i < listener.fingers; i++)
			{		
				//Set up it's position.
				Vector3 tipPosition = new Vector3(0, 0, 0);
				
				//Get a normalized box.
				normalizedBox = listener.frame.InteractionBox;
			
				//Finger coordinates.
				tipPosition.x = normalizedBox.NormalizePoint(listener.frame.Fingers[i].TipPosition).x;
				tipPosition.y = normalizedBox.NormalizePoint(listener.frame.Fingers[i].TipPosition).y;
			
				//Modify coordinates to equal screen resolution.
				tipPosition.x = tipPosition.x * Screen.width;
				tipPosition.y = tipPosition.y * Screen.height;
			
				//Flip Y axis.
				tipPosition.y = tipPosition.y * -1;
				tipPosition.y += Screen.height;

				fingers.Add(new Rect(tipPosition.x, tipPosition.y, 16, 16));
			}
		}
	}
	
	//Member Function: OnGUI/////////////////////////////////////////////////////
	public void OnGUI()
	{
		//Make a note of the current GUI color so that we don't overwrite it.
		Color temp = GUI.color;

		//Retrieve the "special" interface color and use it for the cursors.
		GUI.color = Core.getInstance().interfaceColors.special;
		
		//Place a texture where the cursor currently is.
		foreach (Rect point in fingers)
			GUI.DrawTexture (point, pointerNormal);

		//Restore the original GUI color.
		GUI.color = temp;
	}
}