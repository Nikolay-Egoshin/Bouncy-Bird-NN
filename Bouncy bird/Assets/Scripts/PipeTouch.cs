using UnityEngine.UI;
using UnityEngine;
using System;

public class PipeTouch : MonoBehaviour
{
    [SerializeField] private Transform pipe1Position;
    [SerializeField] private Text scoreText;
    [SerializeField] private AudioSource pointSource;
    private int score;

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*score = Convert.ToInt32(scoreText.text);
        score++;
        scoreText.text = score.ToString();
        if (PlayerPrefs.GetInt("isSound") == 1)
            pointSource.Play();*/
    }

    void Update()
    {
        transform.position = new Vector3(pipe1Position.position[0], pipe1Position.position[1] + 5, 0f);
    }
}
