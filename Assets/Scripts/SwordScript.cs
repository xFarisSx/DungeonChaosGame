using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{

    private bool canDamage = true;
    private float damageDuration;
    private float damageTimer = 0;
    int damage;
    PlayerScript Player;
    string swordType;
    string gunType;
    public Dictionary<string, Sword> swords;
    public Dictionary<string, Gun> guns;
    [SerializeField] Sprite[] sprites;
    public Sprite swordSprite;
    [SerializeField] SpriteRenderer swordVisual;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip clip2;
    [SerializeField] BulletScript bullet;
    [SerializeField] Transform bulletPos;
    Sword newInfo;
    Gun newInfoGun;

    public bool isGun = true;



    private void Awake()
    {
        swords = new Dictionary<string, Sword>() {
            { "First", new Sword{ name="First", damageDuration = 1, damage = 50,swordSprite=sprites[0], enchantCost=1, damageEnchant=0, rewardEnchant=0, reward=2} },
            { "Golden", new Sword{ name="Golden", damageDuration = 0.8f, damage = 70,swordSprite=sprites[1], enchantCost=2, damageEnchant=0, rewardEnchant=0, reward=4} },
            { "Super", new Sword{ name="Super", damageDuration = 0.6f, damage = 100,swordSprite=sprites[2], enchantCost=3, damageEnchant=0, rewardEnchant=0, reward=6} },
            { "Katana", new Sword{ name="Katana", damageDuration = 0.4f, damage = 120,swordSprite=sprites[3], enchantCost=4, damageEnchant=0, rewardEnchant=0, reward=8} },
        };
        guns = new Dictionary<string, Gun>() {
            { "First", new Gun{ name="First", damageDuration = 0.5f, damage = 50,gunSprite=sprites[4], enchantCost=1, damageEnchant=0, rewardEnchant=0, reward=2} }
        };


        Player = GameObject.Find("Player").GetComponent<PlayerScript>();
        

        Initalize();
    }
    // Start is called before the first frame update
    void Start()
    {

        

    }

    public void Initalize()
    {

        if (isGun)
        {
            gunType = Player.gunType;
            for (int i = 0; i < Player.inventory.Guns.Count; i++)
            {
                if (Player.inventory.Guns[i].name == gunType)
                {
                    newInfoGun = Player.inventory.Guns[i];
                }
            }
            if (newInfoGun == null)
            {
                newInfoGun = guns[gunType];
            }

            damage = newInfoGun.damage;
            damageDuration = newInfoGun.damageDuration;
            swordSprite = guns[gunType].gunSprite;

            swordVisual.sprite = swordSprite;
        } else
        {
            swordType = Player.swordType;
            for (int i = 0; i < Player.inventory.Swords.Count; i++)
            {
                if (Player.inventory.Swords[i].name == swordType)
                {
                    newInfo = Player.inventory.Swords[i];
                }
            }
            if (newInfo == null)
            {
                newInfo = swords[swordType];
            }

            damage = newInfo.damage;
            damageDuration = newInfo.damageDuration;
            swordSprite = swords[swordType].swordSprite;

            swordVisual.sprite = swordSprite;
        }
        

    }

    void CheckInputs()
    {
        if (Input.GetMouseButton(0) && canDamage)
        {
            Transform newBullet = Instantiate(bullet.transform, bulletPos.position, transform.rotation);
            audioSource.PlayOneShot(clip2);
            newBullet.GetComponent<BulletScript>().damage = damage;
            canDamage = false;
            damageTimer = 0;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGun)
        {
            CheckInputs();
        }
        if (Player.transform.position.x > transform.position.x)
        {
            if (isGun)
            {
                swordVisual.flipY = true;
            } else
            {
                swordVisual.flipX = true;
            }
            
        }
        if (Player.transform.position.x < transform.position.x)
        {
            if (isGun)
            {
                swordVisual.flipY = false;
            }
            else
            {
                swordVisual.flipX = false;
            }
            
        }
        UpdateTimers();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision != null && !isGun){
            if(collision.gameObject.tag == "Enemy")
            {
                if (canDamage )
                {
                    Debug.Log("wow");
                    MonsterScript enemy = collision.gameObject.GetComponent<MonsterScript>();
                    DamageEnemy(enemy);
                    canDamage = false;
                    damageTimer = 0;
                }
            }
            
        }
    }

    private void DamageEnemy(MonsterScript Enemy)
    {
        Enemy.TakeDamage(damage);
        audioSource.PlayOneShot(clip, 1);
    }

    void UpdateTimers()
    {
        if (damageTimer >= damageDuration)
        {
            canDamage = true;
        }
        else
        {
            damageTimer += Time.deltaTime;
        }
    }
}
public class Sword
{
    public string name;
    public float damageDuration;
    public int damage;
    public Sprite swordSprite;
    public int enchantCost;
    public int damageEnchant;
    public int rewardEnchant;
    public int reward;

}

public class Gun
{
    public string name;
    public float damageDuration;
    public int damage;
    public Sprite gunSprite;
    public int enchantCost;
    public int damageEnchant;
    public int rewardEnchant;
    public int reward;

}