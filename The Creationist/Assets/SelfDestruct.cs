using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelfDestruct : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    [SerializeField] private float lifetime = 1.0f;
    [SerializeField] private bool gametime = false;
    [SerializeField] private bool resetOnMouseOver = false;
    
    [SerializeField] private float beforeDestroyPreDelay = 0.5f;
    [SerializeField] private UnityEvent beforeDestroy;

    private float counter = 0;
    private bool active = true;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!resetOnMouseOver) return;

        active = false;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!resetOnMouseOver) return;

        active = true;
    }

    private void Update()
    {
        if (!active) return;

        if (gametime)
            counter += Time.deltaTime * GameTime.singleton.GameTimeMultipler;
        else counter += Time.deltaTime;

        if(counter >= beforeDestroyPreDelay)
        {
            if (beforeDestroy != null)
                beforeDestroy.Invoke();
        }

        if (counter >= lifetime)
            Destroy(gameObject);
    }

}
