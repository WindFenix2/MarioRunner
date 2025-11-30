using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterMovementController movement;
    [SerializeField] private CharacterVisualSimple visual;
    [SerializeField] private MapMover mapMover;

    [Header("UI")]
    [SerializeField] private GameObject infoUI;
    [SerializeField] private GameObject deadUI;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text deadText;

    [Header("Score")]
    [SerializeField] private float scorePerSecond = 10f;

    [Header("Dev")]
    [SerializeField] private bool autoStartGame = false;
    [SerializeField] private bool keyboardTest = true;

    private bool gameStarted;
    private bool crouching;
    private bool dead;
    private float score;

    private int obstacleLayer;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<CharacterMovementController>();
        if (visual == null) visual = GetComponentInChildren<CharacterVisualSimple>(true);

        obstacleLayer = LayerMask.NameToLayer("Obstacle");
    }

    private void Start()
    {
        ShowInfo();
        if (scoreText != null) scoreText.gameObject.SetActive(true);

        HideDead();

        if (mapMover != null) mapMover.OnPause();

        if (autoStartGame) OnGameStart();
    }

    private void Update()
    {
        if (!keyboardTest) return;

        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.Space)) OnJumpButtonDown();

            if (Input.GetKeyDown(KeyCode.LeftControl)) OnCrouchDown();
            if (Input.GetKeyUp(KeyCode.LeftControl)) OnCrouchUp();

            if (gameStarted)
            {
                float mul = 1f;

                if (mapMover != null)
                {
                    float baseSpeed = Mathf.Max(0.01f, mapMover.MinSpeed);
                    mul = Mathf.Max(1f, mapMover.CurrentSpeed / baseSpeed);
                }

                score += (scorePerSecond * mul) * Time.deltaTime;

                if (scoreText != null)
                    scoreText.text = "Score: " + Mathf.FloorToInt(score);
            }
        }

        if (dead && Input.GetKeyDown(KeyCode.R))
            RestartScene();
    }

    public void OnGameStart()
    {
        if (gameStarted) return;
        gameStarted = true;

        HideInfo();
        HideDead();

        if (mapMover != null)
        {
            mapMover.OnContinue();
            mapMover.OnGameStart();
        }
    }

    public void OnJumpButtonDown()
    {
        if (!gameStarted) OnGameStart();
        if (movement == null) return;

        movement.Jump();
    }

    public void OnCrouchDown()
    {
        if (!gameStarted) OnGameStart();
        if (crouching) return;
        crouching = true;

        if (visual != null) visual.OnCrouchDown();
        if (movement != null) movement.SetCrouch(true);
    }

    public void OnCrouchUp()
    {
        if (!gameStarted) return;
        if (!crouching) return;
        crouching = false;

        if (visual != null) visual.OnCrouchUp();
        if (movement != null) movement.SetCrouch(false);
    }

    public void OnDead()
    {
        if (dead) return;
        dead = true;

        if (mapMover != null) mapMover.OnPause();
        if (movement != null) movement.enabled = false;

        ShowDead();

        int s = Mathf.FloorToInt(score);
        if (deadText != null)
            deadText.text = "You died!\nScore: " + s + "\nPress R to Restart";

        if (scoreText != null) scoreText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dead || !gameStarted) return;
        if (IsObstacle(other)) OnDead();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead || !gameStarted) return;

        if (collision != null && collision.collider != null && IsObstacle(collision.collider))
            OnDead();
    }

    private bool IsObstacle(Collider2D col)
    {
        if (col == null) return false;

        if (obstacleLayer != -1)
        {
            if (col.gameObject.layer == obstacleLayer) return true;
            if (col.transform.root != null && col.transform.root.gameObject.layer == obstacleLayer) return true;
        }

        if (HasTag(col.gameObject, "Obstacle")) return true;
        if (col.transform.root != null && HasTag(col.transform.root.gameObject, "Obstacle")) return true;

        return false;
    }

    private bool HasTag(GameObject go, string tag)
    {
        if (go == null) return false;
        try { return go.CompareTag(tag); }
        catch { return false; }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowInfo() { if (infoUI != null) infoUI.SetActive(true); }
    private void HideInfo() { if (infoUI != null) infoUI.SetActive(false); }
    private void ShowDead() { if (deadUI != null) deadUI.SetActive(true); }
    private void HideDead() { if (deadUI != null) deadUI.SetActive(false); }
}
