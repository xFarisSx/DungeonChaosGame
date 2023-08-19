using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    public TextScript Text;
    private PlayerScript PlayerScript;
    [SerializeField] SwordScript SwordScript;
    [SerializeField] GameObject Falling;
    [SerializeField] SpriteRenderer FallingVisual;
    [SerializeField] Animator animator;
    [SerializeField] ChestVisualScript chestVisualScript;
    [SerializeField] TextScript CostText;

    private List<string> swordTypes;
    List<string> potionTypes;
    public string[] allTypes;

    private bool canF = false;
    private bool activated = false;
    float activateFalseTimer = 0;
    public int duration = 5;
    public int cost = 2;



    void SubscribeToEvents()
    {
        TriggerAreaScript.onPlayerEnter += TriggerAreaScript_onPlayerEnter;
        TriggerAreaScript.onPlayerExit += TriggerAreaScript_onPlayerExit;
    }

    private void TriggerAreaScript_onPlayerExit()
    {
        canF = false;
        Text.HideText();
    }

    private void TriggerAreaScript_onPlayerEnter()
    {
        if (Falling)
        {
            if (!Falling.activeSelf)
            {
                Text.ShowText();
                Text.SetText("Click F To Open The Chest");
                canF = true;
            } else
            {
                TriggerAreaScript_onPlayerExit();
            }
        } else
        {
            TriggerAreaScript_onPlayerExit();
        }
        
        
    }


    // Start is called before the first frame update
    void Start()
    {

        CostText.SetText("$"+cost.ToString());
        PlayerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        SubscribeToEvents();

        swordTypes = new List<string>();
        for (int i = 0; i< SwordScript.swords.Count; i++)
        {
            for(int j = 0; j < SwordScript.swords.Count - i; j++)
            {
                swordTypes.Add(SwordScript.swords.ElementAt(i).Key);
            }
            
        }
        
        potionTypes = new List<string>();
        for (int i = 0; i < PlayerScript.potions.Count; i++)
        {
            potionTypes.Add(PlayerScript.potions.ElementAt(i).Key);
        }

        allTypes = swordTypes.Concat(potionTypes).ToArray();
    }


    // Update is called once per frame
    void Update()
    {
        ActivateFalseAfterTimer();
        if ( canF )
        {
            CheckInputs();
        } 
        if(chestVisualScript.ended && !activated)
        {
            Falling.SetActive(true);
            activated = true;

            animator.SetBool("Open", false);
            
            chestVisualScript.ended = false;
        }
        
    }

    void CheckInputs()
    {
        if(PlayerScript.coins >= cost)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerScript.coins -= cost;
                cost = Convert.ToInt32(cost * 1.5f);
                CostText.SetText("$" + cost.ToString());
                string fallType = GenerateFalling();
                Sprite fallSprite;
                if (SwordScript.swords.Keys.Contains(fallType))
                {

                    Falling.GetComponent<FallingScript>().fallGeneralType = "Sword";
                    fallSprite = SwordScript.swords[fallType].swordSprite;
                    FallingVisual.sprite = fallSprite;
                }
                else if (PlayerScript.potions.Keys.Contains(fallType))
                {
                    Falling.GetComponent<FallingScript>().fallGeneralType = "Potion";
                    fallSprite = PlayerScript.potions[fallType].sprite;
                    FallingVisual.sprite = fallSprite;
                }



                Falling.GetComponent<FallingScript>().fallType = fallType;

                canF = false;
                TriggerAreaScript_onPlayerExit();
                TriggerAreaScript.onPlayerEnter -= TriggerAreaScript_onPlayerEnter;
                TriggerAreaScript.onPlayerExit -= TriggerAreaScript_onPlayerExit;
                animator.SetBool("Open", true);


            }
        }
        
    }
    void ActivateFalseAfterTimer()
    {
        if(activateFalseTimer >= duration)
        {
            activated = false;
            activateFalseTimer = 0;
            TriggerAreaScript.onPlayerEnter += TriggerAreaScript_onPlayerEnter;
            TriggerAreaScript.onPlayerExit += TriggerAreaScript_onPlayerExit;
        } else
        {
            activateFalseTimer += Time.deltaTime;
        }
 
    }

    string GenerateFalling()
    {
        System.Random random = new System.Random();
        int rAll = random.Next(0, allTypes.Length);

        return allTypes[rAll];
    }


}
