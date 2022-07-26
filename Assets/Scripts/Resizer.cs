using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resizer : MonoBehaviour
{
    public float scale = 1.0f;
    public float scaleSpeed = 1.0f;
    public float scaleMin = 0.5f;
    public float scaleMax = 2.0f;

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale, scale, scale), scaleSpeed * Time.deltaTime);
    }

    public void ScaleUp()
    {
        scale = Mathf.Clamp(scale + 0.1f, scaleMin, scaleMax);
    }
    
    public void ScaleDown()
    {
        scale = Mathf.Clamp(scale - 0.1f, scaleMin, scaleMax);
    }
    
    public void Reset()
    {
        scale = 1.0f;
    }
    
    public void SetScale(float newScale)
    {
        scale = Mathf.Clamp(newScale, scaleMin, scaleMax);;
    }

    public void MaxScale()
    {
        scale = scaleMax;
    }
    
    public void MinScale()
    {
        scale = scaleMin;
    }
}
