using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe1 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float deltaSpeed;
    private Vector3 startPosition;
    //[SerializeField] private Player player;
    private float speed1;
    private float height;
    private bool isUpdate = false;

    public float Speed { get => speed; }
    public bool IsUpdate { get => isUpdate; set => isUpdate = value; }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            isUpdate = true;
        }
        if (!isUpdate)
        {
            speed1 += deltaSpeed;
            transform.Translate(Vector2.left * Time.deltaTime * speed1);
        }
        if (isUpdate)
        {
            speed1 = speed;
            transform.position = startPosition;
            isUpdate = false;
        }
        
        if (transform.position.x <= -10) 
        {
            height = Random.Range(-8f, -1.2f);
            transform.localPosition = new Vector3(10f, height, 0f);
        }
    }
}
