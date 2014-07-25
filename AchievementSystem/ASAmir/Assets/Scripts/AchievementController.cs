using UnityEngine;
using System.Collections;

using System.Linq;

public class AchievementController : MonoBehaviour
{
	//the total achievements
	public Achievement[] Achievements;

	//the total badges to be put in scene
	private GameObject[] Badges;

	//the Badge Prefab that will be instantiated
	public GameObject BadgePrefab;

	public AudioClip EarnedSound;
	
	private int currentBadgeValue = 0;
	private int potentialBadgeValue = 0;
	private Vector2 achievementScrollviewLocation = Vector2.zero;


	//in this prototype, a badge is assigned to each achievement
	//the badge is whether enabled (achievement is earned) or it is disabled (achievement not earned yet)
	//this also can be further refined to (golden badge, diamond badge,...)
	private Material BadgeEnabled;
	private Material BadgeDisabled;
	
	private Material StarEnabled;
	private Material StarDisabled;

	private Material NameBGEnabled;
	private Material NameBGDisabled;

	//textmesh wrapper
	static TextMeshWrapper ts;

	void Awake()
	{
		BadgeEnabled = Resources.Load("Materials/BadgeEnabled") as Material;
		BadgeDisabled = Resources.Load("Materials/BadgeDisabled") as Material;
		
		StarEnabled = Resources.Load("Materials/StarEnabled") as Material;
		StarDisabled = Resources.Load("Materials/StarDisabled") as Material;

		NameBGEnabled = Resources.Load("Materials/NameBGEnabled") as Material;
		NameBGDisabled = Resources.Load("Materials/NameBGDisabled") as Material;
	}

	void Start()
	{
		Badges = new GameObject[Achievements.Length] ;

		InstantiateAchievements();
		UpdateReward();

		ts = new TextMeshWrapper ();
	}
	
	// Make sure the setup assumptions we have are met.
	private void InstantiateAchievements()
	{
		ArrayList usedNames = new ArrayList();
		foreach (Achievement achievement in Achievements)
		{
			if (achievement.BadgeValue < 0)
			{
				Debug.LogError("AchievementController::InstantiateAchievements() - Achievement with negative BadgeValue! " + achievement.Name + " gives " + achievement.BadgeValue + " points!");
			}
			
			if (usedNames.Contains(achievement.Name))
			{
				Debug.LogError("AchievementController::InstantiateAchievements() - Duplicate achievement names! " + achievement.Name + " found more than once!");
			}
			usedNames.Add(achievement.Name);
		}

		for (int i = 0; i < Achievements.Length ; i++)
		{
			Badges[i] = Instantiate(BadgePrefab, new Vector3(150 * i , 0, 0), Quaternion.identity) as GameObject;

			//Assign names to the badges
			Transform bdg = Badges[i].gameObject.transform.FindChild("AchievementName").FindChild("Name");
			bdg.GetComponent<TextMesh>().text = 
				gameObject.GetComponent<AchievementController>().Achievements[i].Name;

			//Assign the values to the badges
			Transform bdgValue = Badges[i].gameObject.transform.FindChild("AchievementValue");
			bdgValue.GetComponent<TextMesh>().text = 
				gameObject.GetComponent<AchievementController>().Achievements[i].BadgeValue.ToString();

			//Assign the targetCount (the total number of times the action/event should be triggered)
			Transform targetCnt = Badges[i].gameObject.transform.FindChild("Count").FindChild("TargetCount");
			targetCnt.GetComponent<TextMesh>().text = 
				gameObject.GetComponent<AchievementController>().Achievements[i].TargetCount.ToString();

			//create material and assign it to the badge icon
			Transform bdgIcon = Badges[i].gameObject.transform.FindChild("BadgeIcon");
			//create a new material with a transparent shader
			Material bdgIconMat = new Material(Shader.Find("Unlit/Transparent"));
			//assign the icon to the material main texture
			bdgIconMat.mainTexture = gameObject.GetComponent<AchievementController>().Achievements[i].BadgeIcon;
			//assign the material to the icon
			bdgIcon.gameObject.renderer.material = bdgIconMat;

			//Assign the targetCount (the total number of times the action/event should be triggered)
			Transform descriptionText = Badges[i].gameObject.transform.FindChild("Description").FindChild("DescriptionText");

			//wrap the text
			string textMeshWrapped = 
				ResolveTextSize(gameObject.GetComponent<AchievementController>().Achievements[i].Description,2);

			descriptionText.GetComponent<TextMesh>().text = textMeshWrapped;

		}

	}
	
	private Achievement GetAchievementByName(string achievementName)
	{
		return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
	}
	
	private void UpdateReward()
	{
		currentBadgeValue = 0;
		potentialBadgeValue = 0;
		
		foreach (Achievement achievement in Achievements)
		{
			if (achievement.Earned)
			{
				currentBadgeValue += achievement.BadgeValue;
			}
			
			potentialBadgeValue += achievement.BadgeValue;
		}
	}
	
	private void AchievementEarned()
	{
		UpdateReward();
		AudioSource.PlayClipAtPoint(EarnedSound, Camera.main.transform.position);        
	}
	
	public void AddCountToAchievement(string achievementName, float progressAmount)
	{
		Achievement achievement = GetAchievementByName(achievementName);
		if (achievement == null)
		{
			Debug.LogWarning("Achievement doesn't exist: " + achievementName);
			return;
		}
		
		if (achievement.AddCount(progressAmount))
		{
			AchievementEarned();
		}
	}
	
	public void SetCountToAchievement(string achievementName, float newProgress)
	{
		Achievement achievement = GetAchievementByName(achievementName);
		if (achievement == null)
		{
			Debug.LogWarning("Achievement doesn't exist: " + achievementName);
			return;
		}
		
		if (achievement.SetCount(newProgress))
		{
			AchievementEarned();
		}
	}

	// Update is called once per frame
	void Update ()
	{

		for (int i = 0; i < Achievements.Length ; i++)
		{
			if(Achievements[i].Earned)
			{
				Badges[i].gameObject.transform.FindChild("BadgeTexture").renderer.material = BadgeEnabled;

				Badges[i].gameObject.transform.FindChild("Star").renderer.material = StarEnabled;

				Badges[i].gameObject.transform.FindChild("AchievementName").FindChild("NameBG").renderer.material = NameBGEnabled;
			}
			else
			{
				Badges[i].gameObject.transform.FindChild("BadgeTexture").renderer.material = BadgeDisabled;

				Badges[i].gameObject.transform.FindChild("Star").renderer.material = StarDisabled;

				Badges[i].gameObject.transform.FindChild("AchievementName").FindChild("NameBG").renderer.material = NameBGDisabled;

			}

			
			//Update the currentCount (the actual number of times the action/event has been triggered)
			Transform CurrentCnt = Badges[i].gameObject.transform.FindChild("Count").FindChild("CurrentCount");
			CurrentCnt.GetComponent<TextMesh>().text = 
				gameObject.GetComponent<AchievementController>().Achievements[i].CurrentCount.ToString();
		}
	}





	// Wrap text by line height
	private string ResolveTextSize(string input, int lineLength){
		
		// Split string by char " "
		string[] words = input.Split(" "[0]);
		
		// Prepare result
		string result = "";
		
		// Temp line string
		string line = "";
		
		// for each all words
		foreach(string s in words){
			// Append current word into line
			string temp = line + " " + s;
			
			// If line length is bigger than lineLength
			if(temp.Length > lineLength){
				
				// Append current line into result
				result += line + "\n";
				// Remain word append into new line
				line = s;
			}
			// Append current word into current line
			else {
				line = temp;
			}
		}
		
		// Append last line into result
		result += line;
		
		// Remove first " " char
		return result.Substring(1,result.Length-1);
	}

}