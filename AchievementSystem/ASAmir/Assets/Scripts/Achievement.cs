using UnityEngine;
using System.Collections;

[System.Serializable]
public class Achievement
{
		//the name is also used as an identifier
		public string Name;
		//describe how the Achievement could be earned
		public string Description;
		
		//the value of the earned badge. this is generally expressed in points (coins, stars,...)
		public int BadgeValue;
		
		//a specific icon of the badge.
		public Texture BadgeIcon;
		
		//The targetCount is the level/number/time.. that has to be reached in order to earn the achievement
		//notice that we use floats here so that we can track Time for example
		public float TargetCount;
		// The progress towards the targetCount 
		public float CurrentCount = 0.0f;
		
		//is the achievement already earned or not yet
		public bool Earned = false;
		
		//increment the current count of an achievement by a given amount
		public bool AddCount(float amount)
		{
			//if the achievement is already earned
			if (Earned)
			{
				return false;
			}
			
			CurrentCount += amount;
			
			//if the target count is reached then mark the achievement as earned
			if (CurrentCount >= TargetCount)
			{
				Earned = true;
				return true;
			}
			return false;
		}
		
		//set the current count of an achievement to a given amount
		public bool SetCount(float amount)
		{
			if (Earned)
			{
				return false;
			}
			
			CurrentCount = amount;
			
			//if the target count is reached then mark the achievement as earned
			if (amount >= TargetCount)
			{
				Earned = true;
				return true;
			}
			return false;
		}

}

