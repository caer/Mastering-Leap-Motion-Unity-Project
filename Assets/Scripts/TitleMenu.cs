//File: TitleMenu.cs
//Project: Mastering Leap Motion
//Date: July 24, 2013
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

//Class: TitleMenu\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
//
//This class creates a simple main menu comprised of three TouchableButtons.
//
public class TitleMenu : MonoBehaviour
{
//Public\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

	//Smoothed button height for middle, right, and left.
	public float middleButtonHeight = 0.0f;
	public float leftButtonHeight = 0.0f;
	public float rightButtonHeight = 0.0f;

	//Font size.
	public static int fontsize = 10000;

	//Is the title menu open?
	public bool open = false;
	
	//Touchable buttons.
	public TouchableButton colorButton;
	public TouchableButton playButton;
	public TouchableButton greyButton;

	//Member Function: OnEnable\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	public void OnEnable()
	{
		//The menu is now open.
		open = true;

		//Initialize buttons.
		colorButton = new TouchableButton();
		playButton = new TouchableButton();
		greyButton = new TouchableButton();
	}

	//Member Function: OnDisable\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	public void OnDisable()
	{
		//The menu is now closed.
		open = false;
	}
	
	//Member Function: OnGUI\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
	public void OnGUI() 
	{
		//Set up GUI fonts.
		GUI.skin.button.fontSize = ((Screen.width + Screen.height) / 2) / 15;
		fontsize = GUI.skin.button.fontSize;

		//Set up GUI colors again.
		GUI.color = new Color(Core.getInstance().interfaceColors.primary.r, Core.getInstance().interfaceColors.primary.g, Core.getInstance().interfaceColors.primary.b);

		//Clicking the Play button will unpause the game and begin play.
		if(playButton.render(new Rect(((Screen.width / 2) - (180 * ((Screen.width + Screen.height) / 2) / 1500)), 
		                     (Screen.height / 2 - middleButtonHeight - GUI.skin.button.fontSize), 
		                      360 * ((Screen.width + Screen.height) / 2) / 1500, 180 * ((Screen.width + Screen.height) / 2) / 1500), "play"))
		{
			//Close and disable the menu.
			open = false; enabled = false;
		}

		//Clicking the Color button will restore the interface colorscheme to its defaults.
		if(colorButton.render(new Rect(((Screen.width / 2) - (580 * ((Screen.width + Screen.height) / 2) / 1500)), 
		                      (Screen.height / 2 - leftButtonHeight - GUI.skin.button.fontSize), 
		                       360 * ((Screen.width + Screen.height) / 2) / 1500, 180 * ((Screen.width + Screen.height) / 2) / 1500), "colour"))
		{
			Core.getInstance().interfaceColors.setGreyscale(false);
		}

		//Clicking the Grey button will set the interface colorscheme to grayscale.
		if(greyButton.render(new Rect(((Screen.width / 2) + (220 * ((Screen.width + Screen.height) / 2) / 1500)), 
		                     (Screen.height / 2 - rightButtonHeight - GUI.skin.button.fontSize), 
		                      360 * ((Screen.width + Screen.height) / 2) / 1500, 180 * ((Screen.width + Screen.height) / 2) / 1500), "grey"))
		{
			Core.getInstance().interfaceColors.setGreyscale(true);
		}
	}
}