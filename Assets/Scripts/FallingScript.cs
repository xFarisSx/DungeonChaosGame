using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingScript : MonoBehaviour
{
    [SerializeField] public TextScript Text;
    private bool canF = false;
    private PlayerScript playerScript;
    public string fallType;
    public bool isChestFalling = true;
    public string fallGeneralType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Text.ShowText();
        Text.SetText($"Click F To Collect The {fallGeneralType}");
        canF = true; 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Text.HideText();
        canF = false;
    }
    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
           canF = false;
            
            if(fallGeneralType == "Sword")
            {
                playerScript.UseSword(fallType);
            } else if(fallGeneralType == "Potion")
            {
                playerScript.GetPotion(fallType);
            }
            if (!isChestFalling)
            {
                Destroy(gameObject);
            } else
            {
                gameObject.SetActive(false);
            }
            

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canF)
        {
            CheckInputs();
        }
        
    }
}
