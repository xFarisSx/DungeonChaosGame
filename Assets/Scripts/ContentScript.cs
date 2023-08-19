using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ContentScript : MonoBehaviour
{

    [SerializeField] GameObject InventoryTemplate;
    [SerializeField] PlayerScript player;
    [SerializeField] SwordScript sword;
    Dictionary<string, Sword> swords;
    private bool firstTime;

    // Start is called before the first frame update
    void Start()
    {
        firstTime = true;
        Initialize();
    }
    public void Initialize()
    {

        GameObject template = InventoryTemplate;
        GameObject gO;
         swords = sword.swords;
        
        

        for (int i = 0; i < swords.Count; i++)
        {
            if (firstTime != true)
            {
                gO = transform.GetChild(i+1).gameObject;

            } else
            {
                gO = Instantiate(template, transform);
            }

            gO.SetActive(true);
            gO.transform.GetChild(0).GetComponent<Text>().text = swords.ElementAt(i).Value.name;
            Text text = gO.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            if (player.swordType == swords.ElementAt(i).Value.name)
            {
                text.text = "Equiped";
            }
            else if (player.inventory.Swords.Contains(swords.ElementAt(i).Value))
            {
                text.text = "Equip";
            }
            else
            {
                text.text = "Locked";
            }


        }
        firstTime = false;
        template.SetActive(false);
    }

    void Clear()
    {
        for (int i = 1; i < swords.Count; i++)
        {



        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
