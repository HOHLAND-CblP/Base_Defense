using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float damage;


    float distance;

    public void Initialize(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }


    public float Hit()
    {
        Destroy(gameObject);

        return damage;
    }


    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        distance += speed * Time.deltaTime;
        if (distance > 20)
            Destroy(gameObject);
    }
}
