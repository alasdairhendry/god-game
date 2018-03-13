using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    private List<AI> entityAI = new List<AI>();
    private Vector2 delayRange = new Vector2(2.5f, 5.0f);   // The delay between assigning new AI
    private bool aiIsActive = false;
    [SerializeField] private AI activeAI;

    private bool isEnabled = true;

    private void Start()
    {
        StartCoroutine(AssignAI());
    }

    private IEnumerator AssignAI()
    {
        while (true)
        {
            while (aiIsActive) yield return null;
            while (!isEnabled) yield return null;
            while (entityAI.Count <= 0) yield return null;
            
            yield return GameTime.singleton.Yield(Random.Range(delayRange.x, delayRange.y));

            while (aiIsActive) yield return null;
            while (!isEnabled) yield return null;
            while (entityAI.Count <= 0) yield return null;

            AI _ai = GetPriority();
            if (_ai != null)
            {
                _ai.Begin();
            }
            //else { Debug.LogError("AI is Null"); }
        }
    }

    public void SetActiveAI(AI ai)
    {
        activeAI = ai;
        aiIsActive = true;
    }

    public void StopActiveAI()
    {
        if (activeAI != null)
            activeAI.Stop();    
    }

    public void Play()
    {
        isEnabled = true;
    }

    public void Stop()
    {
        StopActiveAI();
        isEnabled = false;
    }

    public void OnActiveAIFinished(AI sender)
    {
        if (sender != activeAI) return;
        activeAI = null;
        aiIsActive = false;
        GetComponent<Entity>().GetAttributes.Update(Attribute.AttributeKey.status, "Idling");
    }

    private AI GetPriority()
    {
        float highestPriority = 0.0f;
        AI highestPriorityAI = entityAI[0];

        foreach (AI ai in entityAI)
        {
            float c = ai.GetPriority();
            if(c > highestPriority)
            {
                highestPriority = c;
                highestPriorityAI = ai;
            }
        }

        if (highestPriority == 0)
            return null;
        else
            return highestPriorityAI;
    }

    public void AddAIComponent(AI component)
    {
        entityAI.Add(component);
    }

    public void RemoveAIComponent(AI component)
    {
        if(activeAI == component)
        {
            OnActiveAIFinished(component);
        }
        entityAI.Remove(component);
    }	
}
