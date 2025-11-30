using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterMovementController movement;
    [SerializeField] private CharacterVisualSimple visual;
    [SerializeField] private MapMover mapMover;

    [Header("Dev")]
    [SerializeField] private bool autoStartGame = true;
    [SerializeField] private bool keyboardTest = true;

    private bool gameStarted;
    private bool crouching;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<CharacterMovementController>();
        if (visual == null) visual = GetComponentInChildren<CharacterVisualSimple>(true);
    }

    private void Start()
    {
        if (autoStartGame) OnGameStart();
    }

    private void Update()
    {
        if (!keyboardTest) return;

        if (Input.GetKeyDown(KeyCode.Space)) OnJumpButtonDown();

        if (Input.GetKeyDown(KeyCode.LeftControl)) OnCrouchDown();
        if (Input.GetKeyUp(KeyCode.LeftControl)) OnCrouchUp();
    }

    public void OnGameStart()
    {
        if (gameStarted) return;
        gameStarted = true;

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
        if (crouching) return;
        crouching = true;

        if (visual != null) visual.OnCrouchDown();
        if (movement != null) movement.SetCrouch(true);
    }

    public void OnCrouchUp()
    {
        if (!crouching) return;
        crouching = false;

        if (visual != null) visual.OnCrouchUp();
        if (movement != null) movement.SetCrouch(false);
    }

    public void OnDead()
    {
        if (mapMover != null) mapMover.OnPause();
        if (movement != null) movement.enabled = false;
    }
}
