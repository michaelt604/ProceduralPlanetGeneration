using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{

    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)    //Update noise settings
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)    //Evaluate hegith at point
    {
        float noiseValue = 0;
        float frequency = settings.baseFrequency;
        float amplitude = 1;

        for (int i = 0; i < settings.numLayers; i++)    //For each layer
        {
            float v = noise.Evaluate(settings.centre + point * frequency);  //Calculate base height
            noiseValue += (v + 1) * amplitude * 0.5f;       //Modify noise by ampliute
            frequency *= settings.frequency;    //Augment frequency            
            amplitude *= settings.amplitude;  //Augment amplitude
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);  //Ensure value is non negative
        return noiseValue * settings.strength;  //Modify noise by strength
    }
}
