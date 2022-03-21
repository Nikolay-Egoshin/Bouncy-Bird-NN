using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe2 : MonoBehaviour
{
    [SerializeField] private Transform pipe1Position;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(pipe1Position.position[0], pipe1Position.position[1] + 10, 0f);
    }

}
