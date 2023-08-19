using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipButtonScript : MonoBehaviour
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

    public void Equip()
    {
        if (transform.GetChild(0).GetComponent<Text>().text == "Equip")
        {
            player.UseSword(transform.parent.GetChild(0).GetComponent<Text>().text);
            transform.parent.parent.GetComponent<ContentScript>().Initialize();
        }
    }
}
