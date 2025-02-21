using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndFloatIE : MonoBehaviour
{
    //public FallAndFloatConfig config;

    public float horizonForce=0.5f;
    public float dragForce=1;
    public float dropA=2;
    public float cycleTime = 0.77f;
    public float maxVelocity = 0.33f;
    public float torque = 4f;
    public float minY = -5;
    public int hd = 1;  //旋转方向


    private static FallAndFloatIE instance;
    public static FallAndFloatIE getInstance()
    {
        return instance;
    }

    public virtual void Awake()
    {
        instance = this;

        //config = Resources.Load<FallAndFloatConfig>("Config\\FallAndFloatConfig");

        /*if (config != null)
        {
            horizonForce = config.horizonForce;
            dragForce = config.dragForce;
            dropA = config.dropA;
            cycleTime = config.cycleTime;
            maxVelocity = config.maxVelocity;
            torque = config.torque;
            minY = config.minY;
        }*/
    }

    public IEnumerator FallAndFloat(GameObject petal, Rigidbody2D rb)
    {
        int i = 1;
        while (petal.transform.position.y > minY)
        {
            float t = 0;
            while (t < cycleTime)
            {
                Vector2 hForce = hd * horizonForce * Mathf.Log(i + 1) * Vector2.right;
                rb.AddForce(hForce);
                Vector2 vForce = dropA * rb.mass * Vector2.down;
                rb.AddForce(vForce);

                rb.AddTorque(hd * Mathf.Log(i + 1) * (i == 1 ? 0.7f : 1f) * torque * Mathf.Deg2Rad);

                yield return new WaitForFixedUpdate();
                t += Time.fixedDeltaTime;
            }

            while (rb.velocity.magnitude > maxVelocity * Mathf.Log(i + 1))
            {
                rb.AddForce(-dragForce * rb.velocity.normalized);
                rb.AddTorque((rb.angularVelocity > 0 ? -1 : 1) * torque);

                yield return new WaitForFixedUpdate();
                t += Time.fixedDeltaTime;
            }
            hd *= -1;
            i++;
        }

        //后续可能还要添加触地沿x轴旋转效果
        yield break;
    }
}
