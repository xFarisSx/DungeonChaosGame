using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class MonsterScript : MonoBehaviour
{
    public string monsterType;
    private Monster monsterTypeProps;
    [SerializeField] float speed = 2f;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float xMove = 0f;
    private float yMove = 0f;
    [SerializeField] float moveRange;
    [SerializeField] PlayerScript Player;
    [SerializeField] ChestScript chestScript;
    [SerializeField] SwordScript swordScript;
    [SerializeField] GameObject fallingPre;
    private SpriteRenderer fallingVisual;

    private bool canDamage = true;
    private float damageDuration = 1;
    private float damageTimer = 0;
    private float hitRange = 1.3f;
    public float damage = 25f;

    bool canDamaged = true;
        float damagedTimer = 0;
    private float damagedDuration = 0.8f;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBarScript healthBar;

    public Dictionary<string, Monster> monsterTypes = new Dictionary<string, Monster>() { };

    // Start is called before the first frame
    // 

    public void SetMaxHealth(int newHealth)
    {
        maxHealth = newHealth;
       currentHealth = newHealth;
        healthBar.SetMaxHealth(maxHealth);
}
    void Initalize() {
        monsterTypeProps = monsterTypes[monsterType];
        damage = monsterTypeProps.damage;
        damageDuration = monsterTypeProps.damageDuration;
        hitRange = monsterTypeProps.hitRange;
        speed = monsterTypeProps.speed;

    }
    void Start()
    {

        monsterTypes = new Dictionary<string, Monster>() {
            {"Monster" ,  new Monster{ damage = 25f, damageDuration=1, hitRange = 1.3f, speed = 2  } },
            { "BossMonster", new Monster{ damage = 50f, damageDuration=2, hitRange = 1.8f, speed = 1 }}
        };

        Initalize();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
        Animate();
        Move();
        if (!canDamaged) {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(221f / 255f, 29f / 255f, 62f / 255f, 0.8f);
        } else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void UpdateTimers()
    {
        if(damageTimer >= damageDuration)
        {
            canDamage = true;
        } else
        {
            damageTimer += Time.deltaTime;
        }

        if (damagedTimer >= damagedDuration)
        {
            canDamaged = true;
            damagedTimer = 0;
        }
        else
        {
            damagedTimer += Time.deltaTime;
        }
    }

    void Move()
    {
        Vector2 d = Player.transform.position - transform.position;

        if (d.magnitude < moveRange && d.magnitude > hitRange)
        {
            xMove = (Player.transform.position.x - transform.position.x);
            yMove = (Player.transform.position.y - transform.position.y);

            //rb.velocity = new Vector3(xMove, yMove, 0).normalized * speed;
            GetComponent<AIPath>().canMove = true;
        }
        else
        {
            rb.velocity = Vector3.zero;
            xMove = 0;
            yMove = 0;
            GetComponent<AIPath>().canMove = false;
            
            
        }

        if(d.magnitude < hitRange) {
            if (canDamage)
            {
                DamagePlayer();
                canDamage = false;
                damageTimer = 0;
            }
        }

        


    }
    private void DamagePlayer()
    {
        Player.TakeDamage(Convert.ToInt32(damage));
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            KillSelf();
        }
        healthBar.SetHealth(currentHealth);
        canDamaged = false;
        damagedTimer = 0;
    }

    private void KillSelf()
    {
        Player.coins += Player.usedSword.reward;
        SpawnItem();
        Destroy(gameObject);
    }

    void SpawnItem() {
        string fallType = GenerateFalling();
        if (fallType == "Nothing") return;

        GameObject falling = Instantiate(fallingPre, transform.position, Quaternion.identity);
        fallingVisual = falling.transform.GetChild(0).GetComponent<SpriteRenderer>();
        Sprite fallSprite;
        if (swordScript.swords.Keys.Contains(fallType))
        {

            falling.GetComponent<FallingScript>().fallGeneralType = "Sword";
            fallSprite = swordScript.swords[fallType].swordSprite;
            fallingVisual.sprite = fallSprite;
        }
        else if (Player.potions.Keys.Contains(fallType))
        {
            falling.GetComponent<FallingScript>().fallGeneralType = "Potion";
            fallSprite = Player.potions[fallType].sprite;
            fallingVisual.sprite = fallSprite;
        }


        FallingScript fallingScript = falling.GetComponent<FallingScript>();
        fallingScript.fallType = fallType;
        fallingScript.isChestFalling = false;
        fallingScript.Text = Player.Text;
        falling.SetActive(true);
    }

    string GenerateFalling()
    {
        string[] allTypes = chestScript.allTypes;
        System.Random random = new System.Random();
        int r = random.Next(0, 10);
        if (r != 5) return "Nothing";
        int rAll = random.Next(0, allTypes.Length);

        return allTypes[rAll];
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
        if (xMove < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (xMove > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}

public class Monster
{
    public float damage;
    public float damageDuration;
    public float hitRange;
    public float speed;
}