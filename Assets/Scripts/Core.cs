//File: Core.cs
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

//Class: Core\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
//
//The Core class forms the center of the Leap Flying Entity Unity project,
//facilitating communication between different scripts and handling the
//general flow of the user experience.
//
public class Core : BaseSingleton<Core>
{
//Private\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	
	//Leap Listener.
	private LeapListener listener;

//Public\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

	//Interface colorscheme.
	public Colorscheme interfaceColors = new Colorscheme();
	
	//Title menu.
	public TitleMenu titleMenu = null;

	//Does the application have focus?
	public bool applicationFocused = true;

	//Paused?
	public bool paused = true;

	//Member Function: onAwake\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	public override void onAwake()
	{
		//This script will not be destroyed, even when a new level loads.
		DontDestroyOnLoad(gameObject);

		//Create a new Leap Listener.
		listener = new LeapListener();
	}
	
	//Member Function: OnApplicationFocus\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	public void OnApplicationFocus(bool pauseStatus) {applicationFocused = pauseStatus;}
	
	//Member Function: Update\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	public void Update()
	{
		//Update the leap listener.
		listener.refresh();

		//If the user closes their hand while the game is not paused, pause it.
		if (listener.hands < 1 || titleMenu.open || !applicationFocused)
		{	
			//Pause all entities
			paused = true;

			//Open the title menu.
			if (titleMenu.open == false) titleMenu.enabled = true;

			//Hide the hands.
			this.GetComponent<HandRenderer>().enabled = false;
		}
		
		//Otherwise, keep the game running.
		else if (listener.hands >= 1 && titleMenu.open == false)
		{	
			//Unpause all entities.
			paused = false;

			//Show the hands.
			this.GetComponent<HandRenderer>().enabled = true;
		}
	}
}