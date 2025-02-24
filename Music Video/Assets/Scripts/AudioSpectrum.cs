// Unity Audio Spectrum data analysis
// IMDM Course Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class AudioSpectrum : MonoBehaviour
{
    AudioSource source;
    public static int FFTSIZE = 1024; // https://en.wikipedia.org/wiki/Fast_Fourier_transform
    public static float[] samples = new float[FFTSIZE];
    public static float audioAmp = 0f;
    public static float bassAmp = 0f;
    public static float noBassAmp = 0f;
    void Start()
    {
        source = GetComponent<AudioSource>();       
    }
    void Update()
    {
        // The source (time domain) transforms into samples in frequency domain 
        GetComponent<AudioSource>().GetSpectrumData(samples, 0, FFTWindow.Hanning);
        // Empty first, and pull down the value.
        audioAmp = 0f;
        for (int i = 0; i < FFTSIZE; i++)
        {
            audioAmp += samples[i];
        }

        bassAmp = 0f;
        for (int i = 0; i < 9; i++) //only adds up sum of lower frequency samples
        {
            bassAmp += samples[i];
        }

        noBassAmp = 0f;
        for (int i = 9; i < 1024; i++) //adds up frequencies, excluding bass
        {
            noBassAmp += samples[i];
        }          
    }
}
