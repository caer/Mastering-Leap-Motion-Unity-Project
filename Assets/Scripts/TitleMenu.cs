//File: TitleMenu.cs
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

//Class: TitleMenu/////////////////////////////////////////////////////////////
//
//This class creates a simple main menu comprised of three TouchableButtons.
//
public class TitleMenu : MonoBehaviour
{
  //Smoothed button height for middle, right, and left.
  public float buttonHeight = 0.0f;

  //Font size.
  public static int fontsize = 10000;

  //Is the title menu open?
  public bool open = false;
  
  //Touchable buttons.
  public TouchableButton colorButton;
  public TouchableButton playButton;
  public TouchableButton greyButton;

  //Member Function: OnEnable//////////////////////////////////////////////////
  public void OnEnable()
  {
    //The menu is now open.
    open = true;

    //Initialize buttons.
    colorButton = new TouchableButton();
    playButton = new TouchableButton();
    greyButton = new TouchableButton();
  }

  //Member Function: OnDisable/////////////////////////////////////////////////
  public void OnDisable()
  {
    //The menu is now closed.
    open = false;
  }
  
  //Member Function: getButtonRect/////////////////////////////////////////////
  public Rect getButtonRect(float x, float y)
  {
    return new Rect(((Screen.width / 2) - (x * ((Screen.width + Screen.height) / 2) / 1500)), 
                     (Screen.height / 2 - y - GUI.skin.button.fontSize), 
                      360 * ((Screen.width + Screen.height) / 2) / 1500, 180 * ((Screen.width + Screen.height) / 2) / 1500);
  }
  
  //Member Function: OnGUI/////////////////////////////////////////////////////
  public void OnGUI() 
  {
    //Set up GUI fonts.
    GUI.skin.button.fontSize = ((Screen.width + Screen.height) / 2) / 15;
    fontsize = GUI.skin.button.fontSize;

    //Set up GUI colors again.
    GUI.color = new Color(Core.getInstance().interfaceColors.primary.r, Core.getInstance().interfaceColors.primary.g, Core.getInstance().interfaceColors.primary.b);

    //Clicking the Play button will unpause the game and begin play.
    if(playButton.render(getButtonRect(180, buttonHeight), "play"))
    {
      //Close and disable the menu.
      open = false; enabled = false;
    }

    //Clicking the Colour button will restore the interface colorscheme to its defaults.
    if(colorButton.render(getButtonRect(580, buttonHeight), "colour"))
    {
      Core.getInstance().interfaceColors.setGreyscale(false);
    }

    //Clicking the Grey button will set the interface colorscheme to greyscale.
    if(greyButton.render(getButtonRect(-220, buttonHeight), "grey"))
    {
      Core.getInstance().interfaceColors.setGreyscale(true);
    }
  }
}