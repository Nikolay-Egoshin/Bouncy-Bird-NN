using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text soundText;
    [SerializeField] private Text recordText;
    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private AudioSource birdSource;
    private Quaternion rotation;

    private void Start()
    {
        if (PlayerPrefs.HasKey("isSound"))
        {
            if (PlayerPrefs.GetInt("isSound") == 1)
            {
                soundText.text = "Выключить звук";
            }
            else if (PlayerPrefs.GetInt("isSound") == 0)
            {
                soundText.text = "Включить звук";
            }
        }
        else
        {
            PlayerPrefs.SetInt("isSound", 1);
            soundText.text = "Выключить звук";
        }

        if (PlayerPrefs.HasKey("RecordScore"))
        {
            recordText.text = "Ваш рекорд: " + PlayerPrefs.GetInt("RecordScore");
        }
        else
        {
            PlayerPrefs.SetInt("RecordScore", 0);
            recordText.text = "Ваш рекорд: 0";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
            Instantiate(birdPrefab, new Vector3((Input.mousePosition.x - 960) / 107, 6, 90), rotation);
            if (PlayerPrefs.GetInt("isSound") == 1)
                birdSource.Play();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickSound()
    {
        if (PlayerPrefs.GetInt("isSound") == 1)
        {
            PlayerPrefs.SetInt("isSound", 0);
            soundText.text = "Включить звук";
        }
        else if (PlayerPrefs.GetInt("isSound") == 0)
        {
            PlayerPrefs.SetInt("isSound", 1);
            soundText.text = "Выключить звук";
        }
    }
}
