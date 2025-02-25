using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwayAnimate : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject cylinder1;
    
    //For drawing the line
    public LineRenderer midLineRenderer;
    int points = 5000;
    float amplitude;
    float frequency;
    public Vector2 xLimits = new Vector2(0,1);
    public float movementSpeed = 1;
    public float radians;
    
    bool stringBreak;
    bool exposition;
    bool ending;

    void Start()
    {   
        // Start color of the middle line
        midLineRenderer = GetComponent<LineRenderer>();
        midLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        Color startColor = new Color32(33, 158, 188, 1); //Cerulean color
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(startColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        midLineRenderer.colorGradient = gradient;
        
        // Creates the cylinder and scales/rotates it to look like a circle in the center of the screen
        cylinder1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder1.transform.localScale = new Vector3(5f, 0.001f, 5f);
        cylinder1.transform.localRotation = Quaternion.Euler(90f, 0, 0);

        // Attaches material to cylinder
        Material newMat = new Material(Shader.Find("Standard"));
        newMat.color = new Color32(17, 178, 108, 1); //Dark green color
        SetMaterialTransparent(newMat);

        Renderer cylRenderer = cylinder1.GetComponent<Renderer>();
        cylRenderer.material = newMat;

        //RandomObjects();
        RenderSettings.skybox.SetColor("_Tint", Color.black);
    }   

    // Update is called once per frame
    void Update()
    {
       Opaque(cylinder1);
       float cylinderSize = AudioSpectrum.bassAmp*20f + 5f;
       cylinder1.transform.localScale = new Vector3(cylinderSize, 0.001f, cylinderSize);
       DrawLine(midLineRenderer);
       stringBreak = false;
       exposition = false;
       ending = false;

       if (Time.time <= 23.8f) // Exposition
       {            
            exposition = true;
            
       }
       else if (Time.time > 23.8f && Time.time <= 47.6f) // Climax 1
       {
            stringBreak = false;
            exposition = false;
            //String breaks
            if(Time.time > 29.1f && Time.time <= 30.6f)
            {
                stringBreak = true;
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

       else if(Time.time > 71.1f && Time.time <= 86.7f) // Exposition Coda
       {
            exposition = true;
       }

       else if (Time.time > 86.7f && Time.time <= 109f) // Climax 2
       {
            //String breaks
            if(Time.time > 91.1f && Time.time <= 92.9f)
            {
                stringBreak = true;
            }else if (Time.time > 97.8f && Time.time <= 98.8f)
            {
                stringBreak = true;
            }else if (Time.time > 103.1f && Time.time <= 104.7f)
            {
                stringBreak = true;
            }

            //Skybox flashes color at the "hey!"s
            if(Time.time > 93.77f && Time.time <= 97.47f)
            {
                SkyboxFlash();
            }else
            {
                RenderSettings.skybox.SetColor("_Tint", Color.black); //Makes sure skybox is back to black after flashes
            }
       }

       else if (Time.time > 109f && Time.time <= 132.6f) // Falling Action
       {
            //String breaks
            if(Time.time > 115f && Time.time <= 116.7f)
            {
                stringBreak = true;
            }else if (Time.time > 121.7f && Time.time <= 122.85f)
            {
                stringBreak = true;
            }else if (Time.time > 126.7f && Time.time <= 128.4f)
            {
                stringBreak = true;
            }
       }

       else if (Time.time > 132.6f && Time.time <= 140f) // Resolution
       {
            //String breaks
            if(Time.time > 132.6f && Time.time <= 134.9f)
            {
                stringBreak = true;
            }else if (Time.time > 138.6f && Time.time <= 140f)
            {
                stringBreak = true;
                ending = true;
            }
       }

       else if (Time.time > 140f)// After song is over
       {
            Renderer objRenderer = cylinder1.GetComponent<Renderer>();
            objRenderer.material.color = new Color(0, 0, 0, 0);
       }
    }

    public void DrawLine(LineRenderer lineRenderer)
    {
        //Modified from https://youtu.be/6C1NPy321Nk?si=CSSGoafxwgqB9Mpl
        float xStart = xLimits.x;
        float Tau = 2* Mathf.PI;
        float xFinish = xLimits.y;
        movementSpeed = AudioSpectrum.audioAmp;

        if (stringBreak == true)
        {
            frequency = AudioSpectrum.audioAmp / 2f;
            amplitude = AudioSpectrum.audioAmp * 3f;
        }else //when other instruments are playing, line renders at a lower amp/frequency
        {
            frequency = AudioSpectrum.noBassAmp / 2f;
            amplitude = AudioSpectrum.noBassAmp / 2f;
        }
 
        lineRenderer.positionCount = points;
        for(int currentPoint = 0; currentPoint<points;currentPoint++)
        {
            float progress = (float)currentPoint/(points-1);
            float x = Mathf.Lerp(xStart,xFinish,progress);
            float y = amplitude*Mathf.Sin((Tau*frequency*x)+(Time.timeSinceLevelLoad*movementSpeed));
            lineRenderer.SetPosition(currentPoint, new Vector3(x,y,0));
        }
        
        // Changes the line of the color based on certain sections of the song
        Color32 currentColor;
        if (stringBreak == true && ending == false) //During a string break and not the ending of the song
        {
            currentColor = new Color32(70, 0, 211, 1); //Dark blue color
        }else if (exposition == false) //During the climaxes/buildup
        {
            currentColor = new Color32(198, 6, 0, 1); //Dark red/orange color
        }else //During an exposition section
        {
            currentColor = new Color32(33, 158, 188, 1); //Cerulean color
        }

        // Line transparency is reactive to amplitude of music
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(currentColor, 0.0f), new GradientColorKey(currentColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(Mathf.Clamp(AudioSpectrum.noBassAmp, 0, 1), 0.0f), new GradientAlphaKey(Mathf.Clamp(AudioSpectrum.noBassAmp, 0, 1), 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    private void Opaque(GameObject obj) {
        if(obj == null) return;

        Renderer objRenderer = obj.GetComponent<Renderer>();
        if(objRenderer == null || objRenderer.material.color == null) return;
        
        // Changes the color of the cylinder based on certain sections of the song
        Color currentColor;
        if (exposition == false) //During climax/buildup
        {
            currentColor = new Color32(251, 133, 9, 0); //Orange color
        }else //During exposition
        {
            currentColor = new Color32(17, 178, 108, 1); //Dark green color
        }
        
        float targetAlpha = Mathf.Clamp(AudioSpectrum.bassAmp * 5f, 0.01f, 1f);

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

    private void SkyboxFlash()
    {
        Color colorStart = Color.black;
        Color colorEnd = new Color32(255, 183, 3, 1); //light yellow/orange color
        float duration = (20f/27f)/2f; //bpm of the song is 81, so 60/81 (or 20/27) seconds is the length of a beat
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        RenderSettings.skybox.SetColor("_Tint", Color.Lerp(colorStart, colorEnd, lerp));
    }
}
