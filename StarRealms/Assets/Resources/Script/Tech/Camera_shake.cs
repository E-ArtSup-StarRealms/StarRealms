using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_shake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;
    public GameObject Sparks;
    public GameObject Warning_light;
    public GameObject Smoke;
    public GameObject Normal_light;

    void Start()
    {
        Sparks.SetActive(false);
        Warning_light.SetActive(false);
        Smoke.SetActive(false);
        Normal_light.SetActive(true);
    }


    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
            Sparks.SetActive(true);
            Smoke.SetActive(true);
            Normal_light.SetActive(false);
            Warning_light.SetActive(true);
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
}
