#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RunnerTuningWindow : EditorWindow
{
    // ---- DEFAULTS (твои нужные значени€) ----
    private const float DEF_MIN_SPEED = 1f;
    private const float DEF_MAX_SPEED = 2f;
    private const float DEF_BOOST_PER_SEC = 0.1f;
    private const float DEF_NO_BOOST_TIME = 1f;
    private const float DEF_SCORE_PER_SEC = 10f;

    // values in the window
    private float minSpeed = DEF_MIN_SPEED;
    private float maxSpeed = DEF_MAX_SPEED;
    private float boostPerSec = DEF_BOOST_PER_SEC;
    private float noBoostTime = DEF_NO_BOOST_TIME;
    private float scorePerSec = DEF_SCORE_PER_SEC;

    [MenuItem("Tools/Runner Tuning")]
    public static void Open()
    {
        var w = GetWindow<RunnerTuningWindow>("Runner Tuning");
        w.minSize = new Vector2(320, 260);
        w.ResetDefaults(); // <-- чтобы при открытии были нужные дефолты
    }

    private void OnEnable()
    {
        // если окно открыли через Layout/перезапуск Unity Ч тоже пусть будут дефолты
        ResetDefaults();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Runner tuning (Scene)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This changes values on objects in the CURRENT scene.", MessageType.Info);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("MapMover", EditorStyles.boldLabel);

        minSpeed = EditorGUILayout.Slider("Min Speed", minSpeed, 0f, 20f);
        maxSpeed = EditorGUILayout.Slider("Max Speed", maxSpeed, 0f, 20f);
        boostPerSec = EditorGUILayout.Slider("Boost / sec", boostPerSec, 0f, 10f);
        noBoostTime = EditorGUILayout.Slider("No boost time", noBoostTime, 0f, 30f);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Character", EditorStyles.boldLabel);
        scorePerSec = EditorGUILayout.Slider("Score / sec", scorePerSec, 0f, 200f);

        EditorGUILayout.Space(10);

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Load from Scene")) LoadFromScene();
            if (GUILayout.Button("Apply to Scene")) ApplyToScene();
        }

        EditorGUILayout.Space(6);
        if (GUILayout.Button("Reset Defaults")) ResetDefaults();
    }

    private void ResetDefaults()
    {
        minSpeed = DEF_MIN_SPEED;
        maxSpeed = DEF_MAX_SPEED;
        boostPerSec = DEF_BOOST_PER_SEC;
        noBoostTime = DEF_NO_BOOST_TIME;
        scorePerSec = DEF_SCORE_PER_SEC;
        Repaint();
    }

    private void LoadFromScene()
    {
        var map = FindFirstObjectByType<MapMover>();
        if (map != null)
        {
            var so = new SerializedObject(map);
            minSpeed = GetFloat(so, "minSpeed", minSpeed);
            maxSpeed = GetFloat(so, "maxSpeed", maxSpeed);
            boostPerSec = GetFloat(so, "boostSpeedPerSecond", boostPerSec);
            noBoostTime = GetFloat(so, "nonBoostSpeedTime", noBoostTime);
        }

        var ch = FindFirstObjectByType<Character>();
        if (ch != null)
        {
            var so = new SerializedObject(ch);
            scorePerSec = GetFloat(so, "scorePerSecond", scorePerSec);
        }

        Repaint();
    }

    private void ApplyToScene()
    {
        var map = FindFirstObjectByType<MapMover>();
        if (map != null)
        {
            Undo.RecordObject(map, "Apply Runner Tuning (MapMover)");
            var so = new SerializedObject(map);

            SetFloat(so, "minSpeed", minSpeed);
            SetFloat(so, "maxSpeed", maxSpeed);
            SetFloat(so, "boostSpeedPerSecond", boostPerSec);
            SetFloat(so, "nonBoostSpeedTime", noBoostTime);

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(map);
        }

        var ch = FindFirstObjectByType<Character>();
        if (ch != null)
        {
            Undo.RecordObject(ch, "Apply Runner Tuning (Character)");
            var so = new SerializedObject(ch);

            SetFloat(so, "scorePerSecond", scorePerSec);

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(ch);
        }
    }

    private float GetFloat(SerializedObject so, string propName, float fallback)
    {
        var p = so.FindProperty(propName);
        return (p != null && p.propertyType == SerializedPropertyType.Float) ? p.floatValue : fallback;
    }

    private void SetFloat(SerializedObject so, string propName, float value)
    {
        var p = so.FindProperty(propName);
        if (p != null && p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
    }
}
#endif
