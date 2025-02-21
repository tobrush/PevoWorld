using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieMenu : MonoBehaviour
{

	public bool PieMenuOn;
	public List<string> commands;
	public List<Texture> icons;

	public float iconSize = 64f;
	public float spacing = 12f;
	public float speed = 8f;
	public GUISkin skin;

	[HideInInspector]
	public float scale;
	[HideInInspector]
	public float angle;

	PieMenuManager manager;

	void Awake()
	{
		manager = PieMenuManager.Instance;
	}

	void OnMouseUp()
	{
		if(!PieMenuOn)
        {
			manager.Show(this);
			PieMenuOn = true;
		}
		else
        {
			manager.Hide(this);
			PieMenuOn = false;
		}
		

		/*
		 5v5
		
		skin data shop pay
		
		*/ 
	}

}
