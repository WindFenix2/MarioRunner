using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovementController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;

    [Header("Settings")]
    [SerializeField] private float jumpForce = 6f;

    [SerializeField] private float groundCheckDistance = 0.08f;

    [Header("Development settings")]
    [SerializeField] private bool showDetectGroundRay;

    private int groundLayerMask;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (col == null) col = GetComponent<Collider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    public bool IsGround()
    {
        if (col == null) return false;

        var b = col.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y + 0.01f);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayerMask);

        if (showDetectGroundRay)
        {
            Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider ? Color.green : Color.red);
        }

        return hit.collider != null;
    }

    public void Jump()
    {
        if (!IsGround()) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
