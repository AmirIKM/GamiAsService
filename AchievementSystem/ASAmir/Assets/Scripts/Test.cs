using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public AchievementController AchievementController;

    private bool inStealthMode = false;
    private int currentDaysCount = 0;
    private int numRedPotions = 0;
    private int numBluePotions = 0;
    private int numGreenPotions = 0;
    
	void Start()
    {

	}

    private void AnotherSmokingFreeDay()
    {
        currentDaysCount++;
		AchievementController.AddCountToAchievement("1st day", 1.0f);
		AchievementController.AddCountToAchievement("1st week", 1.0f);
    }
	
	void Update()
    {
        if (inStealthMode)
        {
			AchievementController.AddCountToAchievement("The Sports Man", Time.deltaTime);
        }
	}


    void OnGUI()
    {
        float xPosition = 600.0f;

        GUI.Label(new Rect(xPosition, 25.0f, 150.0f, 25.0f), "Smoking-free days: " + currentDaysCount);
        if (GUI.Button (new Rect (xPosition + 150.0f, 25.0f, 100.0f, 25.0f), "Another day!")) 
		{
			AnotherSmokingFreeDay ();
		}
    }
}