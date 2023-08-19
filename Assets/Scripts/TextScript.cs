using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{

    private Text Text;

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        Text.color = color;
    }

    public void SetText(string text)
    {

        if (Text) { Text.text = text; }
    }

    public void ShowText()
    {
        if (Text) { Text.enabled = true; }
    }
    public void HideText()
    {
        if (Text) { Text.enabled = false; }
    }
}
