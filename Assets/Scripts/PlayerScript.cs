using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform sword;
    [SerializeField] Transform swordPosition;
    [SerializeField] SpriteRenderer swordVisual;
    [SerializeField] ContentScript contentScript;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;
    [SerializeField] ContentScript2 contentScript2;
    [SerializeField] ContentScript3 contentScript3;
    [SerializeField] GameObject inventoryUI;
    
    [SerializeField] Sprite[] sprites;
    [SerializeField] TextScript CoinText;
    private Rigidbody2D rb;
    private float xMove;
    private float yMove;
    public int coins = 0;
    private Vector3 dir;
    private RaycastHit2D hit;


    public float mana = 100f;
    public float manaFullingSpeed = 50f;
    public float manaCost = 50f;
    private float manaDuration = 5f;
    private float manaTimer = 0;


    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] HealthBarScript healthBar;
    [SerializeField] ManaBarScript manaBar;
    [SerializeField] public TextScript Text;


    public string swordType = "First";
    public string gunType = "First";
    
    public Sword usedSword;
    public string selectedSword = "First";
    public string selectedGun = "First";

    //public List<Sword> inventory = new List<Sword>() { };

    public Inventory inventory = new Inventory();
    public Dictionary<string, Potion> potions = new Dictionary<string, Potion>() {  };
    SwordScript swordScript;
    [SerializeField] GunScript gunScript;

    private bool canDash = true;
    private float dashDuration = 0.2f;
    private float dashTimer = 0;
    public bool isGun = true;

    public void SelectSword(string type)
    {
        selectedSword = type;
        contentScript3.Initialize();
    }
    private void Awake()
    {
        potions = new Dictionary<string, Potion>() {
            { "Heal", new Potion{ type="Heal", duration = 1, strength = 50,sprite=sprites[2], number = 0} },
            { "Damage", new Potion{ type="Damage", duration = 5, strength = 70,sprite=sprites[1], number = 0, enabled = false, timer = 0} },
            { "Speed", new Potion{ type="Speed", duration = 1, strength = 4,sprite=sprites[0], number = 0, enabled = false, timer = 0} },
        };

        inventory.Potions = potions;
        

    }

    public void GetPotion(string newPotionType)
    {
        inventory.Potions[newPotionType].number += 1;
        contentScript2.Initialize();
    }

    public void UsePotion(string newPotionType)
    {
        inventory.Potions[newPotionType].number -= 1;
        
        if (newPotionType == "Heal")
        {
            currentHealth += inventory.Potions[newPotionType].strength;
            if(currentHealth > maxHealth)
            {
                healthBar.SetMaxHealth(currentHealth);
            }
            healthBar.SetHealth(currentHealth);
        } else
        {
            inventory.Potions[newPotionType].timer = 0;
            inventory.Potions[newPotionType].enabled = true;
            if (newPotionType == "Speed")
            {
                speed += inventory.Potions[newPotionType].strength;

                Debug.Log("Speed");
            }
            if (newPotionType == "Damage")
            {
                for (int i = 0; i < inventory.Swords.Count; i++)
                {
                    if (inventory.Swords[i].name == swordType)
                    {
                        inventory.Swords[i].damage += inventory.Potions[newPotionType].strength;
                        Debug.Log("damage");
                    }
                }
                swordScript.Initalize();
            }
        }
        
        contentScript2.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {

        

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        manaBar.SetMaxMana(100);

        rb = GetComponent<Rigidbody2D>();

        swordScript = sword.gameObject.GetComponent<SwordScript>();
        inventory.Swords.Add(swordScript.swords["First"]);
        inventory.Guns.Add(swordScript.guns["First"]);
        usedSword = swordScript.swords[swordType];
    }

    void ChangeSwordPositionAndRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos)+transform.position - Camera.main.transform.position;
        Vector2 dis = (mousePosition - transform.position);
        dis.Normalize();

        swordPosition.transform.position = Vector2.Lerp(swordPosition.transform.position, dis*1.2f + new Vector2(transform.position.x, transform.position.y), 0.1f);
        sword.transform.position = swordPosition.position;

        if (isGun) {
            float rotation = (Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg);
            sword.transform.rotation = Quaternion.Slerp(sword.transform.rotation, Quaternion.AngleAxis(rotation, Vector3.forward), 0.1f);
        } else
        {
            float rotation = (Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg) - 90f;
            sword.transform.rotation = Quaternion.Slerp(sword.transform.rotation, Quaternion.AngleAxis(rotation, Vector3.forward), 0.1f);
        }
        




    }

    public void Enchant(string type, string sword)
    {
        for (int i = 0; i < inventory.Swords.Count; i++)
        {
            if (inventory.Swords[i].name == sword)
            {
                if(inventory.Swords[i].enchantCost <= coins){
                    if (type == "Damage")
                    {
                        inventory.Swords[i].damage += 50;
                        inventory.Swords[i].damageEnchant += 1;
                    }
                    else
                    {
                        inventory.Swords[i].reward = Convert.ToInt32(inventory.Swords[i].reward * 1.5f);
                        inventory.Swords[i].rewardEnchant += 1;
                    }
                    coins -= Convert.ToInt32(inventory.Swords[i].enchantCost);
                    inventory.Swords[i].enchantCost = Convert.ToInt32((inventory.Swords[i].enchantCost * 1.5f));
                    swordScript.Initalize();
                }
                
            }
        }
    }

    public void UseSword(string newSwordType)
    {
        swordType = newSwordType;
        
        if (!inventory.Swords.Contains(swordScript.swords[newSwordType]))
        {
            inventory.Swords.Add(swordScript.swords[swordType]);
        }
        contentScript.Initialize();

        contentScript3.Initialize();
        usedSword = swordScript.swords[swordType];
        sword.gameObject.GetComponent<SwordScript>().Initalize();
        ConvertGun();
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        healthBar.SetHealth(currentHealth);
    }

    private void FixedUpdate()
    {
        

        transform.position = transform.position + new Vector3(dir.x, dir.y) * speed * Time.fixedDeltaTime;
    }



    // Update is called once per frame
    void Update()
    {
        

        CoinText.SetText("Coins: "+coins); 
        UpdateTimers();
        CheckInputs();
        Animate();
        Move();
        ChangeSwordPositionAndRotation();
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryUI.GetComponent<CanvasGroup>().alpha = inventoryUI.GetComponent<CanvasGroup>().alpha == 1 ? 0 : 1;
            inventoryUI.GetComponent<CanvasGroup>().interactable = inventoryUI.GetComponent<CanvasGroup>().interactable ? false : true;
            inventoryUI.GetComponent<CanvasGroup>().blocksRaycasts = inventoryUI.GetComponent<CanvasGroup>().blocksRaycasts ? false : true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash){
            if(mana >= manaCost)
            {
                speed += 8;
                dashTimer = 0;
                canDash = false;
                audioSource.PlayOneShot(clip, 1);

                mana-=manaCost;
                manaTimer = 0;
                manaBar.SetMana(mana);
            }

            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleGunSword();
        }
    }

    public void ConvertGun()
    {
        isGun = true;
        ToggleGunSword();
    }
    public void ToggleGunSword()
    {
        isGun = !isGun;
        swordScript.isGun = isGun;
        swordScript.Initalize();
        swordVisual.flipX = false;
        swordVisual.flipY= false;

        if (isGun)
        {
            if (transform.position.x > swordScript.transform.position.x)
            {
                swordVisual.flipY = true;
            }
           
        }
    }

    void UpdateTimers()
    {

        if (dashTimer >= dashDuration)
        {
            speed -= 8;
            canDash = true;
            dashTimer = 0;
        }
        else if(!canDash)
        {
            dashTimer += Time.deltaTime;
        }

        if (manaTimer >= manaDuration)
        {
            manaTimer = 0;
            mana += manaFullingSpeed;
            if (mana > 100)
            {
                mana = 100;
            }
            manaBar.SetMana(mana);
        }
        else
        {
            manaTimer += Time.deltaTime;
        }


        for (int j = 1; j< inventory.Potions.Count; j++)
        {
            Potion el = inventory.Potions.ElementAt(j).Value;
            if (el.timer >= el.duration)
            {
                if(el.type == "Speed")
                {
                    speed -= el.strength;
                } else if(el.type == "Damage")
                {
                    for (int i = 0; i < inventory.Swords.Count; i++)
                    {
                        if (inventory.Swords[i].name == swordType)
                        {
                            inventory.Swords[i].damage -= inventory.Potions["Damage"].strength;
                            Debug.Log("damage");
                        }
                    }
                    swordScript.Initalize();
                }
                
                el.enabled = false;
                el.timer = 0;
            }
            else if (el.enabled)
            {
                el.timer += Time.deltaTime;
            }
        }
    }

    void Move()
    {


        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        dir = new Vector3(xMove, yMove, 0).normalized;

        float radius = 0.5f; // Set the radius of the capsule
        float distance = 0.1f; // Set the distance of the cast

        hit = Physics2D.CapsuleCast(transform.position, new Vector2(radius, 0.8f), CapsuleDirection2D.Vertical, 0,new Vector2(dir.x, dir.y),distance);

        if (hit)
        {
            //Debug.Log(currentHealth);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                //TakeDamage(20);
                
            }
        }


        // w key changes value to 1, s key changes value to -1



    }

    void Animate()
    {
        if (xMove != 0 || yMove != 0)
        {
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
        }
        if (xMove == -1)
        {
            spriteRenderer.flipX = true;
        }
        if (xMove == 1)
        {
            spriteRenderer.flipX = false;
        }
    }
}


public class Potion
{
    public string type;
    public int strength;
    public int number;
    public float duration;
    public Sprite sprite;
    public bool enabled;
    public float timer;
}
public class Inventory
{
    public List<Sword> Swords = new List<Sword>() { };
    public List<Gun> Guns = new List<Gun>() { };
    public Dictionary<string, Potion> Potions = new Dictionary<string, Potion>() { };
}