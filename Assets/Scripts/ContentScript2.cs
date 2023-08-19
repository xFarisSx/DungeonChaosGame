using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ContentScript2 : MonoBehaviour
{
    [SerializeField] GameObject InventoryTemplate;
    [SerializeField] PlayerScript player;
    [SerializeField] SwordScript sword;
    Dictionary<string, Potion> potions;
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
        potions = player.inventory.Potions;



        for (int i = 0; i < potions.Count; i++)
        {
            if (firstTime != true)
            {
                gO = transform.GetChild(i + 1).gameObject;

            }
            else
            {
                gO = Instantiate(template, transform);
            }

            gO.SetActive(true);
            gO.transform.GetChild(0).GetComponent<Text>().text = potions.ElementAt(i).Value.type;
            Text text = gO.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            gO.transform.GetChild(2).GetComponent<Image>().sprite = potions.ElementAt(i).Value.sprite;
            gO.transform.GetChild(3).GetComponent<Text>().text = potions.ElementAt(i).Value.number.ToString();

            if (potions.ElementAt(i).Value.number >= 1)
            {
                text.text = "Use";

            } else
            {
                text.text = "Locked";
            }

        }
        firstTime = false;
        template.SetActive(false);
    }

    void Clear()
    {
        for (int i = 1; i < potions.Count; i++)
        {



        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
