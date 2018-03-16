using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarCanvas : MonoBehaviour {

    [SerializeField] private Text currentTimeText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentTimeText.text = GameTime.singleton.DaysToTime(GameTime.singleton.CurrentTimeDays);        
	}
}
