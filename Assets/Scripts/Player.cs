//File: Player.cs
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

//Class: Player////////////////////////////////////////////////////////////////
//
//This class controls the "player" Game Object, translating input from the
//HandController class into motion.
//
public class Player : MonoBehaviour
{
	//Reference to the hand controller.
	HandController controller;

	//Member Function: Start/////////////////////////////////////////////////////
	void Start() 
	{ 
		controller = HandController.getInstance();
	}
	
	//Member Function: Update////////////////////////////////////////////////////
	void Update () 
	{
		//Only move if the game is unpaused.
		if (Core.getInstance ().paused == false) 
		{
			//Transform position forward.
			rigidbody.velocity = transform.forward * (controller.y / -2);

			//Rotate.
			transform.Rotate (0, controller.x * 1.25f * Time.deltaTime, 0, Space.World);
		}

		//If the game is paused, cancel all force vectors.
		else rigidbody.velocity = new Vector3(0, 0, 0);
	}
}
