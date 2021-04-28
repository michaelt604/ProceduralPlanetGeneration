using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject 
{
    public int seed = 100;
    public float planetRadius = 1;
    public float direction = 1;
    public NoiseLayer[] noiseLayers;
    public CraterSettings craterSettings;

    //Crater settings
    [System.Serializable]
    public class CraterSettings //Crater settings
    {
        public int numCraters = 1;  //Number of craters in the moon
        [Range(0.01f, 0.2f)]
        public float craterRadiusMin = 0.01f;   //Crater radius min
        [Range(0.01f, 0.2f)]
        public float craterRadiusMax = 0.15f;   //Crater radius max
        [Range(0.01f, 0.5f)]
        public float rimWidth = 0.7f;   //Rim width
        [Range(0.01f, 0.5f)]
        public float rimSteepness = 0.42f;  //Rim steepness
        [Range(-1f, -0.1f)]
        public float floorHeightMin = -1.0f;    //Floor height min
        [Range(-1f, -0.1f)]
        public float floorHeightMax = -0.5f;    //Floor height max
    }

    [System.Serializable]
    public class NoiseLayer //Noise layer settings
    {
        public bool enabled = true;
        public bool usePrevLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}
