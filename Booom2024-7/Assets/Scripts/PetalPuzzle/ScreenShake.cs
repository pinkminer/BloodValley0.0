using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    //public Camera mainCamera;

    public float shakeDuration = 0.5f; // 抖动持续时间
    public float shakeMagnitude = 0.5f; // 抖动幅度

    public Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        //originalPosition = mainCamera.transform.position;
        originalPosition.z = -10;
    }


    public IEnumerator ShakeCo(Camera mainCamera)
    {
        float t = 0;
        while (t < shakeDuration)
        {
            float randomx = Random.Range(-shakeMagnitude, shakeMagnitude);
            float randomy = Random.Range(-shakeMagnitude, shakeMagnitude);
            Vector3 offset = new Vector3(randomx, randomy, 0);
            mainCamera.transform.position = originalPosition + offset;

            t += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPosition;
        yield break;
    }
}
