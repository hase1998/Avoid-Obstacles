using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    private float scoreMultiplier = 10f;
    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;

    public UIDocument uiDocument;
    public GameObject boosterFlame;
    public float thrustForce = 3f;
    Rigidbody2D rb;
    public GameObject explosionEffect;
    public GameObject borderParent;
    public InputAction moveForward;
    public InputAction lookPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        highScoreText.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        moveForward.Enable();
        lookPosition.Enable(); 
    }


    void Update()
    {
        UpdateScore();
        MovePlayer();
    }

    void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt((elapsedTime * scoreMultiplier));
        scoreText.text = "Score: " + score;
    }

    void MovePlayer()
    {
        if (moveForward.IsPressed())
        {
            // Get the mouse position in world coordinates
            // and calculate the direction to the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());
            mousePos.z = 0; // Set z to 0 for 2D
            Vector2 direction = (mousePos - transform.position).normalized;

            // Apply a force in the direction of the mouse position
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }



        if (moveForward.WasPressedThisFrame())
        {
            boosterFlame.SetActive(true);
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            boosterFlame.SetActive(false);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        borderParent.SetActive(false);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        UpdateHighScore();
        
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateHighScore(){
        if (score > PlayerPrefs.GetFloat("HighScore", 0f))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
        highScoreText.text = "High Score: " + PlayerPrefs.GetFloat("HighScore", 0f);
        highScoreText.style.display = DisplayStyle.Flex;
    }
}
