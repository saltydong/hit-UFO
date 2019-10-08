using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour
{
    public Text TrailText;
    public Text scoreText;
    public Text roundText;

    private int round;

    public GameObject bullet;
    public ParticleSystem explosion;
    public float fireRate = .25f;
    public float speed = 500f;
    public bool isGameOver = false;
    private float nextFireTime;

    private GameInterface gameInterface;
    private SceneController scene;

    public void Awake()
    {
        scene = SceneController.getInstance();
        scene.setUserInterface(this);
    }
    void Start()
    {
        bullet = GameObject.Instantiate(bullet) as GameObject;
        bullet.transform.position = new Vector3(100, 100, 100);
        explosion = GameObject.Instantiate(explosion) as ParticleSystem;
        gameInterface = SceneController.getInstance() as GameInterface;
    }

    public void gameOver()
    {
        isGameOver = true;
        TrailText.text = "GameOver";
    }

    void Update()
    {
        if (Input.GetKeyDown("space") && isGameOver)
        {
            scene.setRound(0);
            scene.nextRound();
            isGameOver = false;
        }
        if (!isGameOver)
        {
            if (gameInterface.isCounting())
            {
                TrailText.text = "Trial " + (gameInterface.getTrial() + 1).ToString();
            }
            else
            {

                gameInterface.MakeEmissionDiskable();
                if (gameInterface.isShooting())
                {
                    TrailText.text = "Trail";
                }

                if (gameInterface.isShooting() && Input.GetMouseButtonDown(0) && Time.time > nextFireTime)
                {
                    acting();
                }
            }
            roundText.text = "Round: " + gameInterface.getRound().ToString();
            scoreText.text = "Score: " + gameInterface.getScore().ToString();

            if (round != gameInterface.getRound())
            {
                round = gameInterface.getRound();
                TrailText.text = "Round " + round.ToString() + " !";
            }
        }
    }
    public void acting()
    {
        nextFireTime = Time.time + fireRate;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(ray.direction * speed, ForceMode.Impulse);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Disk")
        {

            explosion.transform.position = hit.collider.gameObject.transform.position;
            explosion.GetComponent<Renderer>().material.color = hit.collider.gameObject.GetComponent<Renderer>().material.color;
            explosion.Play();

            hit.collider.gameObject.SetActive(false);
        }
        else
        {
            scene.getScoreController().wasteBullet();
        }
    }
}