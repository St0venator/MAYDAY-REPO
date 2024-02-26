using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whirlpoolController : MonoBehaviour
{
    Vector3 targetScale;
    Vector3 originScale;

    public float growthScale = 0.00001f;
    float growthPos = 0;
    // Start is called before the first frame update
    void Start()
    {
        originScale = transform.localScale;

        float randScale = Random.Range(1.5f, 2f);
        targetScale = transform.localScale * randScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(originScale, targetScale, growthPos);

        growthPos += Time.deltaTime * growthScale;
    }
}
