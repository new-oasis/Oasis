using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UITouchTest : MonoBehaviour
{
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement; 
        var button = root.Q("Button");
        Debug.Log(button);
        button.RegisterCallback<ClickEvent>((ev) => Debug.Log("One Clicked!"));
        // button.RegisterCallback<PointerUpEvent>((ev) => Debug.Log("One Clicked!"));
        // button.RegisterCallback<PointerDownEvent>((ev) => Debug.Log("One Clicked!"));
    }


}
