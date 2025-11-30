using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TemplateDestroyer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MapSpawner spawner;

    private void Awake()
    {
        if (!spawner)
            spawner = UnityEngine.Object.FindFirstObjectByType<MapSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("LocationTemplate")) return;

        var templateRoot = other.transform.parent;
        if (!templateRoot) return;

        spawner.DeleteTemplate(templateRoot.gameObject);
    }
}
