using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Player Parametrs")]
    public float maxHP;
    float curHP;

    public float speed;

    bool canMove;
    float moveH, moveV;

    int state;
    bool isOnField;
    bool isOnBase;

    float timeOnBase;

    int amountOfMoney;
    int amountOfMoneyInBackpack;

    [Header("Character Components")]
    public Animator anim;

    [Header("Fire Parametrs")]
    public float rateOfFire;
    public float damage;
    public float bulletSpeed;
    bool canShoot;
    float timeAfterShot;
    public GameObject bulletPref;

    [Header("Spawner")]
    public Spawner spawner;

    [Header("Base")]
    public GameObject base_;


    [Header("UI Components")]
    public Slider healthSlider;
    public TextMeshProUGUI textMoneyAmount;
    public TextMeshProUGUI textMoneyInBackpackAmount;
    [Space]
    public GameObject gameOverPanel;
    public TextMeshProUGUI textMoneyAmountGO;


    //cache
    Camera mainCam;


    void Start()
    {
        curHP = maxHP;

        canMove = true;
        state = 0;

        isOnField = false;
        isOnBase = true;

        canShoot = true;


        healthSlider.maxValue = maxHP;
        healthSlider.value = curHP;
        textMoneyAmount.text = amountOfMoney.ToString();
        textMoneyInBackpackAmount.text = amountOfMoneyInBackpack.ToString();


        mainCam = Camera.main;
    }

    void Update()
    {
        if (!canShoot)
        {
            timeAfterShot += Time.deltaTime;

            if (timeAfterShot >= 60 / rateOfFire)
                canShoot = true;
        }
        


        if (canMove)
        {
            Vector3 move = (Vector3.forward * moveV + Vector3.right * moveH);
            if (move.magnitude > 1)
                move.Normalize();

            if (state != 2 && move.magnitude > 0.7f)
            {
                anim.SetTrigger("Running");
                state = 2;
            }
            else if (state != 1 && move.magnitude > 0.05f && move.magnitude < 0.7f)
            {
                anim.SetTrigger("Walking");
                state = 1;
            }
            else if (state != 0 && move.magnitude <=0.05f)
            {
                anim.SetTrigger("DynIdle");
                state = 0;
            }

            if (move != new Vector3(0, 0, 0))
                transform.rotation = Quaternion.LookRotation(move);


            transform.Translate(Vector3.forward * speed * move.magnitude * Time.deltaTime);
        }

        if(isOnField && canShoot)
        {
            Shoot();
        }

        if (isOnBase && curHP<100)
        {
            timeOnBase += Time.deltaTime;

            if(timeOnBase>=1)
            {
                timeOnBase--;
                curHP += 5;

                if (curHP > 100)
                    curHP = 100;

                healthSlider.value = curHP;
            }
        }
    }


    void Shoot()
    {

        canShoot = false;
        timeAfterShot -= rateOfFire/60;

        GameObject closestEnemy = spawner.ClosestEnemy();

        if (closestEnemy != null)
        {
            GameObject bul = Instantiate(bulletPref, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            bul.transform.LookAt(new Vector3(closestEnemy.transform.position.x, bul.transform.position.y, closestEnemy.transform.position.z));
            bul.GetComponent<Bullet>().Initialize(bulletSpeed, damage);
        }
    }


    public void SetDirection(Vector2 direction)
    {
        moveH = direction.x;
        moveV = direction.y;
    }


    public void TakeDamage(float damage)
    {
        curHP -= damage;
        healthSlider.value = curHP;

        if (curHP <= 0)
            GameOver();
    }


    public void GameOver()
    {
        gameObject.SetActive(false);

        gameOverPanel.SetActive(true);
        textMoneyAmountGO.text = amountOfMoney.ToString();
    }

    public void Restart()
    {
        gameOverPanel.SetActive(false);

        amountOfMoney = 0;
        amountOfMoneyInBackpack = 0;

        textMoneyAmount.text = "0";
        textMoneyInBackpackAmount.text = "0";

        curHP = maxHP;
        healthSlider.value = curHP;

        transform.position = base_.transform.position;

        gameObject.SetActive(true);

        spawner.Restart();
    }


    public bool IsOnField()
    {
        return isOnField;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Base"))
        {
            isOnBase = true;

            amountOfMoney += amountOfMoneyInBackpack;
            textMoneyAmount.text = amountOfMoney.ToString();

            amountOfMoneyInBackpack = 0;
            textMoneyInBackpackAmount.text = "0";
        }

        if (col.CompareTag("Field"))
        {
            isOnField = true;
        }


        if (col.CompareTag("Money"))
        {
            col.GetComponent<Money>().Collect();

            amountOfMoneyInBackpack++;
            textMoneyInBackpackAmount.text = amountOfMoneyInBackpack.ToString();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Field"))
        {
            isOnField = false;
            spawner.PlayerOnBase();
        }

        if (col.CompareTag("Base"))
        {
            isOnBase = false;
        }
    }
}
