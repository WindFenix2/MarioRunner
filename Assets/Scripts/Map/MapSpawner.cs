using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TemplatesLoader))]
public class MapSpawner : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TemplatesLoader templatesLoader;

    [Header("Settings")]
    [Tooltip("Map parent transform.")]
    [SerializeField] private Transform templatesParentTransform;

    [Tooltip("Templates count on scene in one moment.")]
    [SerializeField] private int templatesPoolSize;

    [Tooltip("Template size.")]
    [SerializeField] private Vector3 templateSize;

    [Tooltip("Templates on scene.")]
    [SerializeField] private List<GameObject> spawnedTemplates;

    private void Awake()
    {
        if (this.templatesLoader == null)
            this.templatesLoader = GetComponent<TemplatesLoader>();

        if (this.spawnedTemplates == null)
            this.spawnedTemplates = new List<GameObject>();
    }

    private void Update()
    {
        if (this.spawnedTemplates.Count < this.templatesPoolSize)
        {
            SpawnTemplate();
        }
    }

    private void SpawnTemplate()
    {
        GameObject template = this.templatesLoader.GetRandomTemplate();
        if (template == null) return;

        GameObject spawnedTemplate = Instantiate(template, this.templatesParentTransform);

        Vector3 templatePosition = Vector3.zero;

        if (this.spawnedTemplates.Count > 0)
        {
            GameObject lastSpawnedTemplate = this.spawnedTemplates.Last();
            Vector3 lastSpawnedTemplatePosition = lastSpawnedTemplate.transform.localPosition;
            templatePosition = lastSpawnedTemplatePosition + this.templateSize;
        }

        spawnedTemplate.transform.localPosition = templatePosition;
        this.spawnedTemplates.Add(spawnedTemplate);
    }

    public void DeleteTemplate(GameObject template)
    {
        this.spawnedTemplates.Remove(template);
        Destroy(template);
    }
}
