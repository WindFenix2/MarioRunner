using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TemplatesLoader : MonoBehaviour
{
    [Header("Settings")]

    [Tooltip("Loaded templates.")]
    [SerializeField] private List<GameObject> loadedTemplates = new List<GameObject>();

    [Tooltip("Templates folder name in resources.")]
    [SerializeField] private string templatesFolderName;

    [Tooltip("Template name prefix.")]
    [SerializeField] private string templatePrefix;

    [Tooltip("Templates count in resources folder.")]
    [SerializeField] private int templatesCount;

    public GameObject GetRandomTemplate()
    {
        int templateId = Random.Range(1, this.templatesCount);
        string templateName = this.templatePrefix + templateId;

        if (this.loadedTemplates.Exists(t => t != null && t.name == templateName))
        {
            Debug.Log("{<color=cyan><b>Template Loaded Log</b></color>} => [TemplatesLoader] - (<color=yellow>GetRandomTemplate</color>) -> Template " + templateName + " return from loaded templates.");
            return this.loadedTemplates.Find(t => t != null && t.name == templateName);
        }

        string templateResourcePath = $"{this.templatesFolderName}/{templateName}";
        GameObject loadedTemplate = Resources.Load<GameObject>(templateResourcePath);

        if (loadedTemplate == null)
        {
            Debug.LogError("Template not found in Resources. Path: " + templateResourcePath);
            return null;
        }

        this.loadedTemplates.Add(loadedTemplate);
        Debug.Log("{<color=cyan><b>Template Loaded Log</b></color>} => [TemplatesLoader] - (<color=yellow>GetRandomTemplate</color>) -> Template " + templateName + " loaded from resources.");
        return loadedTemplate;
    }
}
