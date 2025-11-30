using System.Collections;
using UnityEngine;

public class MapMover : MonoBehaviour
{
    [Header("Settings")]

    [Tooltip("Minimum moving speed.")]
    [SerializeField] private float minSpeed = 3f;

    [Tooltip("Maximum moving speed.")]
    [SerializeField] private float maxSpeed = 10f;

    [Tooltip("Current speed.")]
    [SerializeField] private float speed = 3f;

    [Tooltip("Boost moving speed per second.")]
    [SerializeField] private float boostSpeedPerSecond = 0.25f;

    [SerializeField] private float nonBoostSpeedTime = 1f;

    [SerializeField] private bool isPlay;

    private Coroutine speedRoutine;

    public float CurrentSpeed => speed;
    public float MinSpeed => minSpeed;

    private void Awake()
    {
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
    }

    private void Update()
    {
        if (!isPlay) return;

        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private IEnumerator SpeedCounter()
    {
        yield return new WaitForSeconds(nonBoostSpeedTime);

        while (true)
        {
            if (isPlay)
            {
                speed += boostSpeedPerSecond * 0.1f;
                speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnGameStart()
    {
        if (speedRoutine != null) StopCoroutine(speedRoutine);
        speedRoutine = StartCoroutine(SpeedCounter());
    }

    public void OnPause() => isPlay = false;

    public void OnContinue() => isPlay = true;
}
