//File: Colorscheme.cs
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
using System;

//Class: Colorscheme///////////////////////////////////////////////////////////
//
//The Colorscheme class is used to streamline the sharing and managing of colors
//across classes and functions.
//
//This class is marked as serializable to enable direct editing of the 
//public colorscheme values from the Unity Inspector.
//
[Serializable]
public class Colorscheme
{
//Private//////////////////////////////////////////////////////////////////////

  //Original colorscheme that we can revert to when switching 
  //between greyscale and normal colors.
  private Colorscheme original;

  //Is this colorscheme currently set to greyscale?
  private bool greyscale = false;

//Public///////////////////////////////////////////////////////////////////////
  
  //Primary color.
  public Color primary = new Color(0, 0, 0, 0);
  
  //Secondary color.
  public Color secondary = new Color(0, 0, 0, 0);
  
  //Accent color #1.
  public Color primaryAccent = new Color(0, 0, 0, 0);
  
  //Accent color #2.
  public Color secondaryAccent = new Color(0, 0, 0, 0);
  
  //Special color.
  public Color special = new Color(0, 0, 0, 0);

  //Member Function: greyscale/////////////////////////////////////////////////
  public void  setGreyscale(bool grey)
  {
    //Set to greyscale if this colorscheme isn't already greyscale.
    if (grey && greyscale == false)
    {
      //Backup current colors.
      original = (Colorscheme) this.MemberwiseClone();

      //Get the greyscale versions of all colors.
      primary = new Color(primary.grayscale, primary.grayscale, primary.grayscale, primary.a);
      secondary = new Color(secondary.grayscale, secondary.grayscale, secondary.grayscale, secondary.a);
      primaryAccent = new Color(primaryAccent.grayscale, primaryAccent.grayscale, primaryAccent.grayscale, primaryAccent.a);
      secondaryAccent = new Color(secondaryAccent.grayscale, secondaryAccent.grayscale, secondaryAccent.grayscale, secondaryAccent.a);
      special = new Color(special.grayscale, special.grayscale, special.grayscale, special.a);

      //Now greyscale.
      greyscale = true;
    }

    //Remove greyscale.
    else if (grey == false && greyscale)
    {
      //Restore original colors.
      primary = original.primary;
      secondary = original.secondary;
      primaryAccent = original.primaryAccent;
      secondaryAccent = original.secondaryAccent;
      special = original.special;

      //No longer greyscale.
      greyscale = false;
    }
  }
}