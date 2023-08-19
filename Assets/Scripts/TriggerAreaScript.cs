using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaScript : MonoBehaviour
{

    public delegate void OnPlayerEnter();
    public static event OnPlayerEnter onPlayerEnter;

    public delegate void OnPlayerExit();
    public static event OnPlayerExit onPlayerExit;

    public void RaiseOnPlayerEnter()
    {
        if (onPlayerEnter != null)
        {
            onPlayerEnter();
        }
    }

    public void RaiseOnPlayerExit()
    {
        if (onPlayerExit != null)
        {
            onPlayerExit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RaiseOnPlayerEnter();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RaiseOnPlayerExit();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
