using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMovementController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D col;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;

    [Header("Ground detect")]
    [SerializeField] private float groundCheckExtra = 0.05f;
    [SerializeField] private bool showDetectGroundRay;

    [Header("Crouch (hitbox)")]
    [Range(0.2f, 1f)]
    [SerializeField] private float crouchHeightMultiplier = 0.8f;
    [SerializeField] private float crouchExtraOffsetY = 0f;

    private Vector2 baseColSize;
    private Vector2 baseColOffset;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (col == null) col = GetComponent<BoxCollider2D>();

        baseColSize = col.size;
        baseColOffset = col.offset;
    }

    private void Update()
    {
        if (!showDetectGroundRay || col == null) return;

        Vector2 origin = col.bounds.center;
        float rayLen = col.bounds.extents.y + groundCheckExtra;
        Debug.DrawRay(origin, Vector2.down * rayLen, Color.red);
    }

    public bool IsGround()
    {
        int mask = LayerMask.GetMask("Ground");
        Vector2 origin = col.bounds.center;
        float rayLen = col.bounds.extents.y + groundCheckExtra;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLen, mask);
        return hit.collider != null;
    }

    public void Jump()
    {
        if (!IsGround()) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void SetCrouch(bool value)
    {
        if (col == null) return;

        if (value)
        {
            col.size = new Vector2(baseColSize.x, baseColSize.y * crouchHeightMultiplier);
            col.offset = baseColOffset + new Vector2(0f, crouchExtraOffsetY);
        }
        else
        {
            col.size = baseColSize;
            col.offset = baseColOffset;
        }
    }
}
