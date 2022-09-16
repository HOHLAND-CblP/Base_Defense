using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Parametrs")]
    public float health;
    float curHealth;

    [Space]
    public float damage;
    public float atackDelay;
    float timeAfterAtack;
    public float distanceToAtack;

    [Space]
    public float checkSpeed;
    public float speed;

    [Space]
    public int minMoney;
    public int maxMoney;
    public GameObject moneyPref;

    // Fortunes
    bool inAtack = false;


    [Header("Enemy Components")]
    public Animator anim;



    

    public Spawner spawner;


    public Player player;


    float curDist;



    public void Initialize(Spawner spawner, Player player)
    {
        this.spawner = spawner;
        this.player = player;

        transform.Rotate(0, Random.Range(0, 360), 0);
    }


    void Start()
    {
        curHealth = health;

        anim.SetTrigger("Walking");
    }


    void Update()
    {
        DistanceToPlayer(out float distXZ, out float distY);

        Vector3 curPos = transform.position;



        if (player.IsOnField())
        {
            if (inAtack)
            {
                timeAfterAtack += Time.deltaTime;
                if (timeAfterAtack >= atackDelay)
                    inAtack = false;
            }
            else if (distXZ < distanceToAtack)
            {
                Atack();
            }


            LookAt(player.transform);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.forward * checkSpeed * Time.deltaTime);
        }
    }

    void Atack()
    {
        inAtack = true;
        timeAfterAtack = 0;

        player.TakeDamage(damage);
    }

    void DistanceToPlayer(out float distXZ, out float distY)
    {
        Vector3 position = transform.position;
        Vector3 playerPos = player.transform.position;

        distXZ = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(playerPos.x, playerPos.z));
        distY = Mathf.Abs(position.y - playerPos.y);
    }

    void LookAt(Transform target)
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void Death()
    {
        int random = Random.Range(0, 10);
        if (random > 3)
        {
            GameObject money = Instantiate(moneyPref, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
        spawner.EnemyDead(gameObject);
        Destroy(gameObject);
    }
    


    private void OnCollisionEnter(Collision col)
    {
        if (!player.IsOnField())
        {
            transform.Rotate(new Vector3(0, 180, 0));
        }

        if (col.transform.CompareTag("Bullet"))
        {
            Debug.Log("Collision");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Bullet"))
        {
            curHealth -= col.GetComponent<Bullet>().Hit();

            if (curHealth <= 0)
                Death();
        }

        if (col.CompareTag("Base") && !player.IsOnField())
        {
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}