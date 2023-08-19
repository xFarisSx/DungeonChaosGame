using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class EnchantSwordScript : MonoBehaviour
{

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

        swords = sword.swords;

        transform.GetChild(0).GetComponent<Text>().text = player.selectedSword;
        transform.GetChild(1).GetComponent<Text>().text = "$1";
        for (int i = 0; i < player.inventory.Swords.Count; i++)
        {
            if(player.inventory.Swords[i].name == player.selectedSword)
            {
                transform.GetChild(1).GetComponent<Text>().text = "$"+player.inventory.Swords[i].enchantCost.ToString();
                transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = player.inventory.Swords[i].damageEnchant.ToString();
                transform.GetChild(5).transform.GetChild(1).GetComponent<Text>().text = player.inventory.Swords[i].rewardEnchant.ToString();
            }
        }

        transform.GetChild(2).GetComponent<Image>().sprite = swords[player.selectedSword].swordSprite;

        transform.GetChild(3).GetComponent<Image>().sprite = swords[player.selectedSword].swordSprite;

        firstTime = false;
    }

    public void Enchant(string type)
    {
        player.Enchant(type, player.selectedSword);
        Initialize();
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
