using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRangeScript : MonoBehaviour
{

    [SerializeField] TextScript Text;
    private bool entered = false;
    [SerializeField] GameObject enchantUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entered = true;
            Text.ShowText();
            Text.SetText("Click F To Open The Enchantment Menu");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Text.HideText();
            entered = false;
        }
            
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entered = true;
            Text.ShowText();
            Text.SetText("Click F To Open The Enchantment Menu");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && entered)
        {
            enchantUI.GetComponent<CanvasGroup>().alpha = enchantUI.GetComponent<CanvasGroup>().alpha == 1 ? 0 : 1;
            enchantUI.GetComponent<CanvasGroup>().interactable = enchantUI.GetComponent<CanvasGroup>().interactable ? false : true;
            enchantUI.GetComponent<CanvasGroup>().blocksRaycasts = enchantUI.GetComponent<CanvasGroup>().blocksRaycasts ? false : true;
        }
    }
}
