using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwayAnimate : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject sphere;

    public LineRenderer myLineRenderer;
    int points = 5000;
    float amplitude;
    float frequency;
    public Vector2 xLimits = new Vector2(0,1);
    public float movementSpeed = 1;
    [Range(0,2*Mathf.PI)]
    public float radians;

    void Start()
    {   
        myLineRenderer = GetComponent<LineRenderer>();
        
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
    }

    // Update is called once per frame
    void Update()
    {
       if (Time.time <= 23.8f) // Exposition
       {
            sphere.transform.position = new Vector3(Mathf.Sin(Time.time), 0f, 0f);
            DrawLine();
       }

       else if (Time.time > 23.8f && Time.time <= 47.6f) // Climax 1
       {
            sphere.transform.position = new Vector3(0f, Mathf.Sin(Time.time), 0f);

            //String breaks
            if(Time.time > 29.1f && Time.time <= 30.6f)
            {
                DrawLine();
            }else if (Time.time > 35.6f && Time.time <= 36.5f)
            {
                DrawLine();
            }else if (Time.time > 40.9f && Time.time <= 42.4f)
            {
                DrawLine();
            }else if (Time.time > 46.8f && Time.time <= 47.6f)
            {
                DrawLine();
            }
            
       }

       else if (Time.time > 47.6f && Time.time <= 71.1f) // Development
       {
            sphere.transform.position = new Vector3(Mathf.Sin(Time.time), Mathf.Sin(Time.time), 0f);
       }

       else if(Time.time > 71.1f && Time.time <= 86.7f) // Exposition Coda
       {
            sphere.transform.position = new Vector3(Mathf.Sin(Time.time), 0f, 0f);
       }

       else if (Time.time > 86.7f && Time.time <= 109f) // Climax 2
       {
            sphere.transform.position = new Vector3(0f, Mathf.Sin(Time.time), 0f);

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
            sphere.transform.position = new Vector3(Mathf.Sin(Time.time), Mathf.Sin(Time.time), 0f);
       }

       else if (Time.time > 132.6f && Time.time <= 140f) // Resolution
       {
            sphere.transform.position = new Vector3(0f, 0f, Mathf.Sin(Time.time));
       }

       else // After song is over
       {
            sphere.transform.position = new Vector3(0f, 0f, 0f);
       }
    }

    public void DrawLine()
    {
        //Modified from https://youtu.be/6C1NPy321Nk?si=CSSGoafxwgqB9Mpl
        float xStart = xLimits.x;
        float Tau = 2* Mathf.PI;
        float xFinish = xLimits.y;

        frequency = AudioSpectrum.audioAmp / 2f;
        amplitude = AudioSpectrum.audioAmp * 2f;
 
        myLineRenderer.positionCount = points;
        for(int currentPoint = 0; currentPoint<points;currentPoint++)
        {
            float progress = (float)currentPoint/(points-1);
            float x = Mathf.Lerp(xStart,xFinish,progress);
            float y = amplitude*Mathf.Sin((Tau*frequency*x)+(Time.timeSinceLevelLoad*movementSpeed));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x,y,0));
        }
    }
}
