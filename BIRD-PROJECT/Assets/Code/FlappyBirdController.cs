using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FlappyBirdController : MonoBehaviour
{

    public GameObject Bird;
    public GameObject PipePrefab;
    public GameObject WingsLeft;
    public GameObject WingsRight;
    public Text ScoreText;
    public TextMeshProUGUI GameOver;

    public float Gravity = 30;
    public float Jump = 10;
    public float PipeSpawnInterval = 2;
    public float PipesSpeed = 5;

    private float VerticalSpeed;
    private float PipeSpawnCountdown;
    private GameObject PipesHolder;
    private int PipeCount;
    private int Score;

    // Start is called before the first frame update
    void Start()
    {

        // Code Ref for start and game over text from a youtube video and unity challenge 5

        // reset score
        Score = 0;
        ScoreText.text = "SCORE: " + Score.ToString();

        // reset pipes
        PipeCount = 0;
        Destroy(PipesHolder);
        PipesHolder = new GameObject("PipesHolder");
        PipesHolder.transform.parent = this.transform;

        // reset bird
        VerticalSpeed = 0;
        Bird.transform.position = Vector3.up * 5;

        // reset time
        PipeSpawnCountdown = 0;

    }


    // Update is called once per frame
    void Update()
    {

        // SMovement
        VerticalSpeed += -Gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            VerticalSpeed = 0;
            VerticalSpeed += Jump;
        }

        Bird.transform.position += Vector3.up * VerticalSpeed * Time.deltaTime;

        //  Pipes
        PipeSpawnCountdown -= Time.deltaTime;

        if (PipeSpawnCountdown <= 0)
        {
            PipeSpawnCountdown = PipeSpawnInterval;

            // create pipe
            GameObject pipe = Instantiate(PipePrefab);
            pipe.transform.parent = PipesHolder.transform;
            pipe.transform.name = (++PipeCount).ToString();

            // pipe position
            pipe.transform.position += Vector3.right * 12;
            pipe.transform.position += Vector3.up * Mathf.Lerp(4, 9, Random.value);

           

        }


        // move pipes left
        PipesHolder.transform.position += Vector3.left * PipesSpeed * Time.deltaTime;


        // Bird animation

        // nose dive
        float speedTo01Range = Mathf.InverseLerp(-10, 10, VerticalSpeed);
        float noseAngle = Mathf.Lerp(-30, 30, speedTo01Range);
        Bird.transform.rotation = Quaternion.Euler(Vector3.forward * noseAngle) * Quaternion.Euler(Vector3.up * 20);

        // wings
        float flapSpeed = (VerticalSpeed > 0) ? 30 : 5;
        float angle = Mathf.Sin(Time.time * flapSpeed) * 45;
        WingsLeft.transform.localRotation = Quaternion.Euler(Vector3.left * angle);
        WingsRight.transform.localRotation = Quaternion.Euler(Vector3.right * angle);

        //  Score
        foreach (Transform pipe in PipesHolder.transform)
        {

            // when pipe has passed the bird
            if (pipe.position.x < 0)
            {
                int pipeId = int.Parse(pipe.name);
                if (pipeId > Score)
                {
                    Score = pipeId;
                    ScoreText.text = "SCORE: " + Score.ToString();
                }
            }

            // when pipe is offscreen
            if (pipe.position.x < -12)
            {
                Destroy(pipe.gameObject);
            }
        }

    }

    // Game over
    public void gameOver()
    {
        GameOver.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void OnTriggerEnter(Collider collider)
    {


        //  Collision
        Start();
        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad"))
        {
            gameOver();
        }
    }
} 
