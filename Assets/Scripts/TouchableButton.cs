//File: TouchableButton.cs
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

//Class: TouchableButton///////////////////////////////////////////////////////
//
//This class creates and manages a "touchable" GUI Button, designed for use with
//the TouchPointer class.  This button will automatically detect TouchPointers
//so long as the [render] method is called within a loop.
//
public class TouchableButton
{
//Private//////////////////////////////////////////////////////////////////////
	
	//Pointer reference.
	private TouchPointer pointer;

	//Stopwatch used to measure hovertime.
	private System.Diagnostics.Stopwatch hoverTime;
	
	//Number of possible missed reads from a hovering finger.
	private int mistakes = 0;
	
	//Current size multiplier being applied to this button.
	private float size = 1.0F;
	
	//Member Function: valueInRange//////////////////////////////////////////////
	//
	//Returns true if the passed item is within the min and max values, and false
	//otherwise.
	//
	private bool valueInRange(float item, float min, float max) { return (item >= min) && (item <= max); }
	
	//Member Function: over//////////////////////////////////////////////////////
	//
	//Returns true if [a]'s X/Y coordinates are within [b], and false otherwise.
	//
	private bool over(Rect a, Rect b)
	{
		bool xOverlap = valueInRange(a.x, b.x, b.x + b.width) ||
			            valueInRange(b.x, a.x, a.x + a.width);
		
		bool yOverlap = valueInRange(a.y, b.y, b.y + b.height) ||
			            valueInRange(b.y, a.y, a.y + a.height);
		
		return xOverlap && yOverlap;
	}
	
//Public///////////////////////////////////////////////////////////////////////
	
	//Time in milliseconds that a finger must hover over this button in
	//order to trigger it.
	public int triggerTime = 750;
	
	//Constructor////////////////////////////////////////////////////////////////
	public TouchableButton()
	{
		//Grab a pointer reference.
		if (pointer == null)
			pointer = TouchPointer.getInstance();
		
		//Setup the stopwatch.
		hoverTime = new System.Diagnostics.Stopwatch();
	}
	
	//Member Function: render////////////////////////////////////////////////////
	//
	//Renders this button on-screen.
	//
	public bool render(Rect location, string text)
	{	
		//Has the button been pressed by the mouse?
		if (GUI.Button(new Rect((location.x - ((location.width * size) / 2)) + (location.width / 2), 
								(location.y - ((location.height * size) / 2)) + (location.width / 2), 
		                    	location.width * size, location.height * size), text))
			return true;
		
		//Has a bad value been read during iteration?
		bool bad = true;
		
		//Is the button being pressed by a LEAP pointer?
		foreach (Rect rect in pointer.fingers)
		{
			//If a finger is over this button, begin counting the amount of time it spends.
			if (over(location, rect))
			{
				//Begin logging hovertime.
				if (hoverTime.IsRunning == false)
					hoverTime.Start();
				
				//Check to see if the hovertime is greater than the trigger time.
				else if (hoverTime.ElapsedMilliseconds > triggerTime)
				{
					//Reset the hovertime.
					hoverTime.Stop();
					hoverTime.Reset();
					
					//Reset the size.
					size = 1.0F;
					
					//Return true.
					return true;
				}
				
				//Increment size.
				size += 0.005F;
				
				//We received at least one good value.
				bad = false;
			}
		}
		
		//If no good values were received, and the hovertime clock is running, increment mistakes.
		if (bad && hoverTime.IsRunning) mistakes += 1;
		
		//Otherwise, reset the mistakes.
		else if (!bad && hoverTime.IsRunning) mistakes = 0;
		
		//If our mistakes exceed 5, stop the hovertime counter.
		if (mistakes >= 5)
		{
			hoverTime.Stop();
			hoverTime.Reset();
			
			size = 1.0F;
		}
		
		//If nothing is pressing the button, return false.
		return false;
	}
}