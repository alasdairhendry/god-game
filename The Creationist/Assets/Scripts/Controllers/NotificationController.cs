using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour {

    public static NotificationController singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }
    
    private Queue<Notification> queue = new Queue<Notification>();

    void Start () {
        StartCoroutine(MonitorQueue());
	}
	
    public void Add(Notification notification)
    {
        queue.Enqueue(notification);
    }

    private IEnumerator MonitorQueue()
    {
        while(true)
        {
            while (queue.Count <= 0) yield return null;

            Notification notification = queue.Dequeue();
            GameObject go = Instantiate(Resources.Load("UI\\Notification_Prefab")) as GameObject;
            go.transform.SetParent(GameObject.Find("Notification_Canvas").transform.Find("Mask_Panel").transform.Find("Notification_Panel"));
            go.transform.Find("Icon_Image").GetComponent<Image>().sprite = notification.Icon;
            go.transform.Find("Header_Text").GetComponent<Text>().text = notification.Header;
            go.transform.Find("Message_Text").GetComponent<Text>().text = notification.Message;
            go.GetComponent<Button>().onClick.AddListener(() => { notification.InvokeAction(); Destroy(go); } );
        }
    }
}
