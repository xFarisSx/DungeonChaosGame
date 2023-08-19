using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    Vector3 rotation;
    public int damage;
    public Vector3 dir;
    public int speed = 20;

    private float damageDuration = 0.2f;
    private float damageTimer = 0;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<MonsterScript>().TakeDamage(damage);
            
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<MonsterScript>().TakeDamage(damage);

        }
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos) + transform.position - Camera.main.transform.position;
        Vector2 dis = (mousePosition - transform.position);
        dir = dis.normalized;

        GetComponent<Rigidbody2D>().velocity = dir * speed;
    }

    void Move()
    {
        //transform.position += dir * speed * Time.deltaTime;
        
    }

    void UpdateTimer()
    {
        if (damageTimer >= damageDuration)
        {
            Destroy(gameObject);
        }
        else
        {
            damageTimer += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }
}
