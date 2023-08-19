using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtonScript : MonoBehaviour
{
    [SerializeField] PlayerScript player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Selected()
    {
        Debug.Log(2);
        if (transform.GetChild(0).GetComponent<Text>().text == "Select")
        {
            player.SelectSword(transform.parent.GetChild(0).GetComponent<Text>().text);
            transform.parent.parent.GetComponent<ContentScript3>().Initialize();
        }
    }
}
