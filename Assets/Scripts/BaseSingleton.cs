//File: BaseSingleton.cs
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

//Class: BaseSingleton/////////////////////////////////////////////////////////
//
//Singleton wrapper for the Base class.  Any class that inherits from this
//will become a singleton.
//
//Instances of this class will still be destroyed upon level changes.  To avoid
//this, call [DontDestroyOnLoad] from within the [OnAwake] callback.
//
public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	//Private////////////////////////////////////////////////////////////////////
	
	//Instance of this type.
	private static T instance;
	
	//Has this type already been enabled?
	private static bool awoken = false;
	
	//Public/////////////////////////////////////////////////////////////////////
	
	//Member Function: Awake/////////////////////////////////////////////////////
	public void Awake()
	{
		//If there's already an instance, delete this one.
		if (instance != null && instance != this)
			Destroy(this);
		
		//Otherwise, proceed to initialize.
		else
		{	
			//Check if we should wake up.
			if (!awoken)
			{	
				//Awake will now no longer be called.
				awoken = true;
				
				//Wake up.
				onAwake();
			}
		}
	}

	//Member Function: onAwake///////////////////////////////////////////////////
	//
	//[onAwake] is called when this object wakes up.  Use this instead of
	//          [Awake].
	//
	//[onAwake] is guaranteed to be called only once if the class implementing
	//          it inherits from BaseSingleton instead of Base.
	//
	public virtual void onAwake() {}
	
	//Member Function: getInstance///////////////////////////////////////////////
	//
	//[getInstance] returns the current active instance.
	//
	//If no active instance exists, this function will attempt to find
	//any instances registered to game objects.  If that fails, this function
	//will create a new instance.
	//
	public static T getInstance()
	{
		//If there is no instance to return, generate a new one.
		if (instance == null)
		{
			//First attempt to see if there's already an instance
			//attached to an object and use that.
			try
			{
				instance = (T) Object.FindObjectOfType(typeof(T));
			}
			
			//Otherwise, create a new object with an instance.
			catch
			{
				GameObject instanceObject = new GameObject(typeof(T).ToString());
				instanceObject.AddComponent<T>();
				instance = instanceObject.GetComponent<T>();
			}
			
			//Call the instance's awake.
			if (!awoken)
			{
				awoken = true;
				
				instance.Invoke("onAwake", 0.0F);
			}
		}
		
		//Return the current instance.
		return instance;
	}
}
