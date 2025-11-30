using UnityEngine;

public class CharacterVisualSimple : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CharacterMovementController movement;

    [Header("Sprites")]
    [SerializeField] private Sprite mouthClosed;
    [SerializeField] private Sprite mouthOpen;

    [Header("Crouch (visual only)")]
    [Range(0.2f, 1f)]
    [SerializeField] private float crouchScaleY = 0.6f;

    [SerializeField] private float crouchOffsetY = -0.08f;

    private bool isCrouch;
    private Vector3 baseScale;
    private Vector3 baseLocalPos;

    private void Awake()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        if (!movement) movement = GetComponentInParent<CharacterMovementController>();

        baseScale = transform.localScale;
        baseLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (!sr || !movement) return;

        bool onGround = movement.IsGround();
        if (mouthClosed && mouthOpen)
            sr.sprite = onGround ? mouthClosed : mouthOpen;

        var scale = baseScale;
        var pos = baseLocalPos;

        if (isCrouch)
        {
            scale.y *= crouchScaleY;
            pos.y += crouchOffsetY;
        }

        transform.localScale = scale;
        transform.localPosition = pos;
    }

    public void OnCrouchDown() => isCrouch = true;
    public void OnCrouchUp() => isCrouch = false;
}
