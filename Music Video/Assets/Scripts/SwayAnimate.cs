using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwayAnimate : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject cylinder1;
    
    //For line
    public LineRenderer myLineRenderer;
    int points = 5000;
    float amplitude;
    float frequency;
    public Vector2 xLimits = new Vector2(0,1);
    public float movementSpeed = 1;
    [Range(0,2*Mathf.PI)]
    public float radians;
    bool stringBreak;

    void Start()
    {   
        myLineRenderer = GetComponent<LineRenderer>();
        //myLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //myLineRenderer.startColor = new Color32(255, 255, 255, 0);
        //myLineRenderer.endColor = new Color32(255, 255, 255, 0);
        
        cylinder1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder1.transform.localScale = new Vector3(5f, 0.001f, 5f);
        cylinder1.transform.localRotation = Quaternion.Euler(90f, 0, 0);

        Material newMat = new Material(Shader.Find("Standard"));
        newMat.color = new Color32(234, 75, 81, 1); //orange color
        SetMaterialTransparent(newMat);

        Renderer cylRenderer = cylinder1.GetComponent<Renderer>();
        cylRenderer.material = newMat;

        RandomObjects();

    }   

    // Update is called once per frame
    void Update()
    {
       if (Time.time <= 23.8f) // Exposition
       {            
            Opaque(cylinder1);
            float cylinderSize = AudioSpectrum.bassAmp*20f + 5f;
            cylinder1.transform.localScale = new Vector3(cylinderSize, 0.001f, cylinderSize);

            stringBreak = false;
            DrawLine();
       }

       else if (Time.time > 23.8f && Time.time <= 47.6f) // Climax 1
       {
            Opaque(cylinder1);  
            float cylinderSize = AudioSpectrum.bassAmp*20f + 5f;
            cylinder1.transform.localScale = new Vector3(cylinderSize, 0.001f, cylinderSize);

            stringBreak = false;
            DrawLine();

            //String breaks
            if(Time.time > 29.1f && Time.time <= 30.6f)
            {
                stringBreak = true;
                //DrawLine();
            }else if (Time.time > 35.6f && Time.time <= 36.5f)
            {
                stringBreak = true;
            }else if (Time.time > 40.9f && Time.time <= 42.4f)
            {
                stringBreak = true;
            }else if (Time.time > 46.8f && Time.time <= 47.6f)
            {
                stringBreak = true;
            }
            
       }

       else if (Time.time > 47.6f && Time.time <= 71.1f) // Development
       {
            Opaque(cylinder1);
            
            float cylinderSize = AudioSpectrum.bassAmp*20f + 5f;
            cylinder1.transform.localScale = new Vector3(cylinderSize, 0.001f, cylinderSize);

            stringBreak = false;
            DrawLine();
       }

       else if(Time.time > 71.1f && Time.time <= 86.7f) // Exposition Coda
       {
            Opaque(cylinder1);
       }

       else if (Time.time > 86.7f && Time.time <= 109f) // Climax 2
       {
            Opaque(cylinder1);

            //Accordion
            if (Time.time > 86.7f && Time.time <= 91.1f)
            {
                // Accordion sound animation here
            }
            
            //String breaks
            if(Time.time > 91.1f && Time.time <= 92.9f)
            {
                DrawLine();
            }else if (Time.time > 97.8f && Time.time <= 98.8f)
            {
                DrawLine();
            }else if (Time.time > 103.1f && Time.time <= 104.7f)
            {
                DrawLine();
            }
       }

       else if (Time.time > 109f && Time.time <= 132.6f) // Falling Action
       {
            Opaque(cylinder1);
       }

       else if (Time.time > 132.6f && Time.time <= 140f) // Resolution
       {
            Opaque(cylinder1);
       }

       else // After song is over
       {
            Opaque(cylinder1);
       }
    }

    public void DrawLine()
    {
        //Modified from https://youtu.be/6C1NPy321Nk?si=CSSGoafxwgqB9Mpl
        float xStart = xLimits.x;
        float Tau = 2* Mathf.PI;
        float xFinish = xLimits.y;

        if (stringBreak == true)
        {
            frequency = AudioSpectrum.audioAmp * 2f;
            amplitude = AudioSpectrum.audioAmp * 50f;
        }else //when other instruments are playing, line renders at a lower amp/frequency, and the movements are more subtle
        {
            frequency = AudioSpectrum.noBassAmp / 5f;
            amplitude = AudioSpectrum.noBassAmp / 4f;
        }
 
        myLineRenderer.positionCount = points;
        for(int currentPoint = 0; currentPoint<points;currentPoint++)
        {
            float progress = (float)currentPoint/(points-1);
            float x = Mathf.Lerp(xStart,xFinish,progress);
            float y = amplitude*Mathf.Sin((Tau*frequency*x)+(Time.timeSinceLevelLoad*movementSpeed));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x,y,0));
        }
    }

    private void Opaque(GameObject obj) {
        if(obj == null) return;

        Renderer objRenderer = obj.GetComponent<Renderer>();
        if(objRenderer == null || objRenderer.material.color == null) return;

        Color currentColor = objRenderer.material.color;

        float targetAlpha = Mathf.Clamp(AudioSpectrum.bassAmp * 5f, 0.1f, 1f);

        objRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }

    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 3); // Set to Transparent mode
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000; // Transparent queue
    }

    private void RandomObjects()
    {
        GameObject[] objects = new GameObject[8];

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objects[i].transform.position = new Vector3 (UnityEngine.Random.Range(-5f,5f), UnityEngine.Random.Range(-5f,5f), 0f);
        }
    }
}
