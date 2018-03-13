using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour {

    public static GameTime singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }
    
    public float DeltaDayTime { get { return (Time.deltaTime * gameTimeMultiplier) / secondsPerDay; } }
    public float DeltaTime { get { return Time.deltaTime * gameTimeMultiplier; } }
    
    private float currentTime = 0.0f;
    public float CurrentTimeDays { get { return currentTime; } }
    private float currentDay = 1;
    private float currentYear = 1;

    [SerializeField] private float secondsPerDay = 10.0f;
    [SerializeField] private float currentSeconds = 0.0f;
    [SerializeField] private float gameTimeMultiplier = 1.0f;
    public float GameTimeMultipler { get { return gameTimeMultiplier; } }
	
	// Update is called once per frame
	void Update () {
        currentSeconds += Time.deltaTime * gameTimeMultiplier;
        currentTime += Time.deltaTime * gameTimeMultiplier;

        if(currentSeconds >= secondsPerDay)
        {
            currentSeconds = 0.0f;
            currentDay++;

            if(currentDay > 365)
            {
                currentDay = 1;
                currentYear++;
            }
        }
	}

    public string DaysToTime(float days)
    {
        string returnValue = "";

        if (days / 372.0f > 1)
        {
            returnValue += Mathf.FloorToInt(days / 372.0f) + " " + Mathf.FloorToInt(days / 365.0f).PluraliseString("Year", "Years");
            float daysToRemove = Mathf.Floor(days / 372.0f) * 372.0f;
            days -= daysToRemove;
        }
        if (days / 31 > 1)
        {
            if (returnValue.Contains("Year")) returnValue += " ";
            returnValue += Mathf.FloorToInt(days / 31.0f) + " " + Mathf.FloorToInt(days / 31.0f).PluraliseString("Month", "Months");
            float daysToRemove = Mathf.Floor(days / 31.0f) * 31.0f;
            days -= daysToRemove;
        }
        if (days > 1)
        {
            if (returnValue.Contains("Year") || returnValue.Contains("Month")) returnValue += " ";
            returnValue += Mathf.FloorToInt(days) + " " + Mathf.FloorToInt(days).PluraliseString("Day", "Days");
        }

        return returnValue;
    }

    public void SetGameTimeMultiplier(float multi)
    {
        gameTimeMultiplier = Mathf.Clamp(multi, 1.0f, 500.0f);       
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(16, 16, 256, 32), "Current Day: " + currentDay.ToString());
    }

    public IEnumerator Yield(float seconds)
    {
        float x = 0;

        while(x < seconds)
        {
            x += Time.deltaTime * gameTimeMultiplier;
            yield return null;
        }
    }
}
