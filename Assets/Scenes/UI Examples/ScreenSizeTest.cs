using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenSizeTest : MonoBehaviour
{
    void Start()
    {
        var panelSettings = GetComponent<UIDocument>().panelSettings; 
        var root = GetComponent<UIDocument>().rootVisualElement; 

        root.Q("Container").RegisterCallback<ClickEvent>((ev) => 
        {
            var r = GetComponent<UIDocument>().rootVisualElement; 
            Debug.Log($"Root: {r.layout}");

            Debug.Log($"-------");

             Debug.Log("ev.position " + ev.position);
            Debug.Log("panelSettings.referenceResolution " + panelSettings.referenceResolution);
            Debug.Log("Screen " +  Screen.width + " x " + Screen.height );

            var computedTouchPosition = new Vector2(
                (ev.position.x / panelSettings.referenceResolution.x) * Screen.width,
                (ev.position.y / panelSettings.referenceResolution.y) * Screen.height
            );
            Debug.Log("computed: " + computedTouchPosition);
        });


        var screenSize = new Vector2(Screen.width, Screen.height);
        Debug.Log($"Screen size: {screenSize}");

        var referenceResolution = panelSettings.referenceResolution;
        Debug.Log($"Reference resolution: {panelSettings.referenceResolution}");

    }


}
