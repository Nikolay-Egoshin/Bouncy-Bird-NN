using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float force;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioSource hitSource;
    /*[SerializeField] private Text scoreText;
    [SerializeField] private Text scoreText1;
    [SerializeField] private Text recordScoreText;
    [SerializeField] private GameObject gameOver;*/
    private GameObject[] pipes;
    private bool isCollision;
    private int score = 0;
    private bool isLose;
    private float spawnTime;
    private GameObject nearestPipe;
    private GameObject nearestPipeTrigger;
    private int nearestPipeNumber = 0;
    private NeuralNetwork brain;
    private bool isCooldown = false;

    public float fitness { get; private set; }
    public int Score { get => score; }
    public bool IsLose { get => isLose; }
    public NeuralNetwork Brain { get => brain; }

    void Start()
    {
        spawnTime = Time.time;
        isLose = false;
        pipes = Camera.main.GetComponent<Manager>().Pipes;
        nearestPipe = pipes[nearestPipeNumber].transform.GetChild(0).gameObject;
        nearestPipeTrigger = pipes[nearestPipeNumber].transform.GetChild(2).gameObject;

        isCollision = false;
        //scoreText.enabled = true;
        //gameOver.SetActive(false);
        /*if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            transform.position = new Vector3(-4.3f, 0f, 0f);
        }
        else if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) 
        {
            transform.position = new Vector3(-1.4f, 0f, 0f);
        }*/
    }

    void Update()
    {
        if (isCollision)
        {
            isLose = true;
            isCollision = false;

            Camera.main.GetComponent<Manager>().BirdCrashed();
            /*scoreText.enabled = false;
            gameOver.SetActive(true);
            score = Convert.ToInt32(scoreText.text);
            if (PlayerPrefs.GetInt("RecordScore") < score)
            {
                PlayerPrefs.SetInt("RecordScore", score);
            }
            scoreText1.text = "Текущий счёт: " + score;
            recordScoreText.text = "Ваш рекорд:" + PlayerPrefs.GetInt("RecordScore");
            if (PlayerPrefs.GetInt("isSound") == 1)
                hitSource.Play();*/
        }

        if (!isLose)
        {
            brain.fitness = Time.time - spawnTime;
            UseNeuralNetwork();
        }
    }

    public void SetBrain(NeuralNetwork brain)
    {
        this.brain = brain;
        brain.fitness = 0;
    }

    private void UseNeuralNetwork()
    {
        float[] inputs = new float[4];
        inputs[0] = nearestPipeTrigger.transform.position.x;
        inputs[1] = nearestPipeTrigger.transform.position.y;
        inputs[2] = nearestPipe.GetComponent<Pipe1>().Speed;
        inputs[3] = transform.position.y;

        var output = brain.FeedForward(inputs);
        if (output[0] > 0 && !isCooldown)
            Jump();
    }

    private void Jump()
    {
        if (isLose == false)
        {
            animator.SetTrigger("jump");
            rigidbody.AddForce(force * Vector2.up, ForceMode2D.Impulse);
            if (PlayerPrefs.GetInt("isSound") == 1)
                jumpSource.Play();
            StartCoroutine("Cooldown");
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(0.3f);
        isCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isCollision = true;
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (nearestPipeNumber == 3)
            nearestPipeNumber = 0;
        else nearestPipeNumber++;
        nearestPipe = pipes[nearestPipeNumber].transform.GetChild(0).gameObject;
        nearestPipeTrigger = pipes[nearestPipeNumber].transform.GetChild(2).gameObject;

        score++;
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene(1);
    }
}
