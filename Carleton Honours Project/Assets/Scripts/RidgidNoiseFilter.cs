using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter 
{
    NoiseSettings.RidgidNoiseSettings settings;
    Noise noise = new Noise();

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings settings)    //Update noise settings
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)    //Evaluate height at point
    {
        float noiseValue = 0;
        float frequency = settings.baseFrequency;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numLayers; i++)    //For each layer
        {
            float v = Mathf.Pow(1 - Mathf.Abs(noise.Evaluate(settings.centre + point * frequency)), 2); //Calculate base height and square to make more distinct ridges
            weight = Mathf.Clamp01(v * settings.weightMultiplier);  //Weight multiplier between 0-1
            v *= weight;    //Multiply height by our weight
            noiseValue += v * amplitude;    //Modify noise by ampliute
            frequency *= settings.frequency;    //Augment frequency
            amplitude *= settings.amplitude;  //Augment amplitude
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue); //Ensure value is non negative
        return noiseValue * settings.strength;  //Modify noise by strength
    }
}