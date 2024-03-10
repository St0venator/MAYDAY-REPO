using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squidBehavior : MonoBehaviour
{
    [SerializeField]private Vector3 swimP; 
    [SerializeField]private Vector3 swimP1;
    [SerializeField]private Vector3 swimP2;

    void Start()
    {
        StartCoroutine(Swimming());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Swimming()
    {
        var swimP = transform.position;
        yield return StartCoroutine(Swim(transform, swimP, swimP1, 3.0f));
        yield return StartCoroutine(Swim(transform, swimP1, swimP, 3.0f));
        while(true)
        {
            yield return StartCoroutine(Swim(transform, swimP, swimP1, 3.0f));
            yield return StartCoroutine(Swim(transform, swimP1, swimP, 3.0f));
        } 
    }

    IEnumerator Swim(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f/time;
        while(i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }
}