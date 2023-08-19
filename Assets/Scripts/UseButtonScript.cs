using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseButtonScript : MonoBehaviour
{
    [SerializeField] PlayerScript player;
    [SerializeField] SwordScript sword;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use()
    {
        if (transform.GetChild(0).GetComponent<Text>().text == "Use")
        {
            player.UsePotion(transform.parent.GetChild(0).GetComponent<Text>().text);
            transform.parent.parent.GetComponent<ContentScript2>().Initialize();
        }
    }
}
