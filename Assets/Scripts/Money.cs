using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [Header("Money Parametrs")]
    public float amplitude;
    public float ampSpeed;
    public float rotationSpeed;

    float startPositionY;




    public void Collect()
    {
        Destroy(gameObject);
    }


    void Start()
    {
        startPositionY = transform.position.y;
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * ampSpeed, amplitude) + startPositionY, transform.position.z);
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
