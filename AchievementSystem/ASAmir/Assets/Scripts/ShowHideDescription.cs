using UnityEngine;
using System.Collections;

public class ShowHideDescription : MonoBehaviour {

	public bool showDescription = false; 
	Transform description;

	// Use this for initialization
	void Start ()
	{
		description = gameObject.transform.FindChild ("Description");
	}
	
	// Update is called once per frame
	void Update ()
	{
		// When you click, change the variables value
		if (showDescription)
						description.gameObject.SetActive(true);
		else
						description.gameObject.SetActive(false);
	}

	void OnMouseEnter()
	{
		showDescription = true;
	}

	void OnMouseExit()
	{
		showDescription = false;
	}
}
