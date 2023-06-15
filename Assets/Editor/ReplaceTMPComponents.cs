using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class ReplaceTMPComponents : EditorWindow
{
    private bool isReversed = false;

    [MenuItem("Tools/Replace TMP Components")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ReplaceTMPComponents));
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace TMP Components", EditorStyles.boldLabel);

        if (GUILayout.Button(isReversed ? "Restore TMP Components" : "Replace Components"))
        {
            if (isReversed)
            {
                RestoreComponents();
            }
            else
            {
                ReplaceComponents();
            }

            isReversed = !isReversed;
        }
    }

    private void ReplaceComponents()
    {
        TMP_Text[] tmpTextComponents = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text tmpText in tmpTextComponents)
        {
            GameObject gameObject = tmpText.gameObject;

            // Create a new GameObject for the Text component
            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(gameObject.transform, false);
            textObject.transform.localPosition = Vector3.zero;
            textObject.transform.localRotation = Quaternion.identity;
            textObject.transform.localScale = Vector3.one;

            // Add Unity UI Text component to the new GameObject
            Text textComponent = textObject.AddComponent<Text>();
            textComponent.text = tmpText.text;
            textComponent.color = tmpText.color;
            textComponent.alignment = TextAnchor.MiddleRight; // Right-to-left alignment
            textComponent.fontSize = Mathf.RoundToInt(tmpText.fontSize);
            textComponent.fontStyle = (FontStyle)tmpText.fontStyle;
            textComponent.raycastTarget = tmpText.raycastTarget;

            // Copy RectTransform properties
            RectTransform tmpRectTransform = tmpText.GetComponent<RectTransform>();
            RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchorMin = tmpRectTransform.anchorMin;
            textRectTransform.anchorMax = tmpRectTransform.anchorMax;
            textRectTransform.pivot = tmpRectTransform.pivot;
            textRectTransform.anchoredPosition = tmpRectTransform.anchoredPosition;
            textRectTransform.sizeDelta = tmpRectTransform.sizeDelta;

            // Remove TMP_Text component
            DestroyImmediate(tmpText);
        }

        TextMeshProUGUI[] tmpButtonComponents = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI tmpButton in tmpButtonComponents)
        {
            GameObject gameObject = tmpButton.gameObject;

            // Create a new GameObject for the Button component
            GameObject buttonObject = new GameObject("Button");
            buttonObject.transform.SetParent(gameObject.transform, false);
            buttonObject.transform.localPosition = Vector3.zero;
            buttonObject.transform.localRotation = Quaternion.identity;
            buttonObject.transform.localScale = Vector3.one;

            // Add Unity UI Button component to the new GameObject
            Button buttonComponent = buttonObject.AddComponent<Button>();

            // Copy RectTransform properties
            RectTransform tmpRectTransform = tmpButton.GetComponent<RectTransform>();
            RectTransform buttonRectTransform = buttonObject.GetComponent<RectTransform>();
            buttonRectTransform.anchorMin = tmpRectTransform.anchorMin;
            buttonRectTransform.anchorMax = tmpRectTransform.anchorMax;
            buttonRectTransform.pivot = tmpRectTransform.pivot;
            buttonRectTransform.anchoredPosition = tmpRectTransform.anchoredPosition;
            buttonRectTransform.sizeDelta = tmpRectTransform.sizeDelta;

            // Remove TextMeshProUGUI component
            DestroyImmediate(tmpButton);
        }

        Debug.Log(isReversed ? "Restored TMP components." : "TMP components replaced with Unity UI components.");
    }

    private void RestoreComponents()
    {
        Text[] textComponents = FindObjectsOfType<Text>();
        foreach (Text textComponent in textComponents)
        {
            GameObject gameObject = textComponent.gameObject;

            // Create a new GameObject for the TMP_Text component
            GameObject tmpObject = new GameObject("TMP_Text");
            tmpObject.transform.SetParent(gameObject.transform, false);
            tmpObject.transform.localPosition = Vector3.zero;
            tmpObject.transform.localRotation = Quaternion.identity;
            tmpObject.transform.localScale = Vector3.one;

            // Add TMP_Text component to the new GameObject
            TMP_Text tmpText = tmpObject.AddComponent<TMP_Text>();
            tmpText.text = textComponent.text;
            tmpText.color = textComponent.color;
            tmpText.alignment = (TextAlignmentOptions)textComponent.alignment;
            tmpText.fontSize = textComponent.fontSize;
            tmpText.fontStyle = (FontStyles)textComponent.fontStyle;
            tmpText.raycastTarget = textComponent.raycastTarget;

            // Copy RectTransform properties
            RectTransform textRectTransform = textComponent.GetComponent<RectTransform>();
            RectTransform tmpRectTransform = tmpObject.GetComponent<RectTransform>();
            tmpRectTransform.anchorMin = textRectTransform.anchorMin;
            tmpRectTransform.anchorMax = textRectTransform.anchorMax;
            tmpRectTransform.pivot = textRectTransform.pivot;
            tmpRectTransform.anchoredPosition = textRectTransform.anchoredPosition;
            tmpRectTransform.sizeDelta = textRectTransform.sizeDelta;

            // Remove Text component
            DestroyImmediate(textComponent);
        }

        Button[] buttonComponents = FindObjectsOfType<Button>();
        foreach (Button buttonComponent in buttonComponents)
        {
            GameObject gameObject = buttonComponent.gameObject;

            // Create a new GameObject for the Button component
            GameObject tmpObject = new GameObject("TMP_Button");
            tmpObject.transform.SetParent(gameObject.transform, false);
            tmpObject.transform.localPosition = Vector3.zero;
            tmpObject.transform.localRotation = Quaternion.identity;
            tmpObject.transform.localScale = Vector3.one;

            // Add Button component to the new GameObject
            Button tmpButton = tmpObject.AddComponent<Button>();

            // Copy RectTransform properties
            RectTransform buttonRectTransform = buttonComponent.GetComponent<RectTransform>();
            RectTransform tmpRectTransform = tmpObject.GetComponent<RectTransform>();
            tmpRectTransform.anchorMin = buttonRectTransform.anchorMin;
            tmpRectTransform.anchorMax = buttonRectTransform.anchorMax;
            tmpRectTransform.pivot = buttonRectTransform.pivot;
            tmpRectTransform.anchoredPosition = buttonRectTransform.anchoredPosition;
            tmpRectTransform.sizeDelta = buttonRectTransform.sizeDelta;

            // Remove Button component
            DestroyImmediate(buttonComponent);
        }

        Debug.Log("Restored TMP components.");
    }
}
