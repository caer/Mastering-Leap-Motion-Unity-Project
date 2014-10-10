//File: HandRenderer.cs
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

//Class: HandRenderer//////////////////////////////////////////////////////////
//
//This class renders 3D interpretations of every single hand
//that is currently within the Leap's field of view into the
//game world.
//
class HandRenderer : MonoBehaviour 
{
//Private//////////////////////////////////////////////////////////////////////
  
  //Leap listener.
  private LeapListener listener;
  
  //Leap box.
  private Leap.InteractionBox normalizedBox;

  //Currently active fingers.
  private GameObject[] fingers;
  
  //Currently active palms.
  private GameObject[] hands;
  
//Public///////////////////////////////////////////////////////////////////////
  
  //Camera to render the hands on.
  public Camera camera = null;
  
  //Finger object.
  public GameObject fingerTip = null;
  
  //Palm object.
  public GameObject palm = null;
  
  //Distance modifiers.
  public float depth = 20.0F;
  public float verticalOffset = -20.0F;
  
  //OnEnable///////////////////////////////////////////////////////////////////
  public void OnEnable() {listener = new LeapListener();}
  
  //OnDisable//////////////////////////////////////////////////////////////////
  public void OnDisable()
  {
    //Reset the hands array.
    if (hands != null)
      for (int i = 0; i < hands.Length; i++)
        Destroy(hands[i]);
    
    //Reset the fingers array.
    if (fingers != null)
      for (int i = 0; i < fingers.Length; i++)
        Destroy(fingers[i]);
  }
  
  //Update/////////////////////////////////////////////////////////////////////
  public void Update()
  {   
    if (listener == null) listener = new LeapListener();

    //Update the listener.
    listener.refresh();
    
    //Get a normalized box.
    normalizedBox = listener.frame.InteractionBox;
    
    //First, get any hands that are present.
    if (listener.hands > 0)
    {
      //Reset the hands array.
      if (hands != null)
        for (int i = 0; i < hands.Length; i++)
          Destroy(hands[i]);
      
      //Initialize our hands.
      hands = new GameObject[listener.hands];
      
      //Loop over all hands.
      for (int i = 0; i < listener.hands; i++)
      {
        try
        {
          //Create a new hand.
          hands[i] = (GameObject) Instantiate(palm);
          
          //Set its properties.
          hands[i].transform.parent = camera.transform;
          hands[i].name = "Palm:" + i;
          
          //Set up it's position.
          Vector3 palmPosition = new Vector3(0, 0, 0);
          
          palmPosition.x += listener.frame.Hands[i].PalmPosition.x / 10;
          palmPosition.y += verticalOffset; palmPosition.y += listener.frame.Hands[i].PalmPosition.y / 10;
          palmPosition.z += depth; palmPosition.z += (listener.frame.Hands[i].PalmPosition.z * -1) / 10;
          
          //Move the hand.
          hands[i].transform.localPosition = palmPosition;
          
          //Setup the hands rotation to neutral.
          Quaternion lr = hands[i].transform.rotation;
          
          Vector3 leap = listener.rotation(listener.frame.Hands[i]);
          
          lr.eulerAngles = new Vector3(leap.x * -1, leap.y, leap.z);
          
          hands[i].transform.localRotation = lr;
        }
        
        //Ignore any array index out of bounds errors.
        catch{}
      }
    }
    
    //If there aren't any. delete any active palms.
    else if (hands != null)
      for (int i = 0; i < hands.Length; i++)
        Destroy(hands[i]);
    
    //Get any fingers that are present.
    if (listener.fingers > 0 && listener.hands > 0)
    {
      //Reset the fingers array.
      if (fingers != null && listener.fingers != fingers.Length)
        for (int i = 0; i < fingers.Length; i++)
          Destroy(fingers[i]);
      
      //Initialize our fingers.
      if (fingers == null || listener.fingers != fingers.Length)
        fingers = new GameObject[listener.fingers];
      
      //Loop over all fingers.
      for (int i = 0; i < listener.fingers; i++)
      {   
        try
        {
          //Create a new finger.
          if (fingers[i] == null)
            fingers[i] = (GameObject) Instantiate(fingerTip);
          
          //Set its properties.
          fingers[i].name = "Finger:" + i;
          fingers[i].transform.parent = camera.transform;
          
          //Set up it's position.
          Vector3 tipPosition = new Vector3(0, 0, 0);
          
          tipPosition.x += listener.frame.Fingers[i].TipPosition.x / 10;
          tipPosition.y += verticalOffset; tipPosition.y += listener.frame.Fingers[i].TipPosition.y / 10;
          tipPosition.z += depth; tipPosition.z += (listener.frame.Fingers[i].TipPosition.z * -1) / 10;
          
          //Move the finger to where it belongs.
          fingers[i].transform.localPosition = tipPosition;
          
          //Setup the hands rotation to neutral.
          Quaternion lr = fingers[i].transform.rotation;
          
          lr.eulerAngles = Vector3.zero;
          
          fingers[i].transform.localRotation = lr;
        }
        
        //Ignore any array index out of bounds errors.
        catch {}
      }
    }
    
    //If not, delete any active fingers.
    else if (fingers != null)
      for (int i = 0; i < fingers.Length; i++)
        Destroy(fingers[i]);
  }
}