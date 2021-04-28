using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;
    public List<float> layerValues = new List<float>();
    public List<Vector3> craters = new List<Vector3>();
    public List<float> cratersRadius = new List<float>();
    public List<float> cratersFloor = new List<float>();

    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)   //Create all the noise layers
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);  //Add noise layer to the list
        }
        elevationMinMax = new MinMax(); //New minmax
        Random.InitState(settings.seed);    //Set seed from settings

        //Crater creation code
        float rangeMin = -1.0f;
        float rangeMax = 1.0f;
        if (craters.Count != settings.craterSettings.numCraters)    //If number of craters changed, reupdate the craters
        {
            craters.Clear();
            cratersRadius.Clear();
            cratersFloor.Clear();
            for (int i = 0; i < settings.craterSettings.numCraters; i++)    //Enter list entities for each of the crater settings
            {
                craters.Add(new Vector3(Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax)));
                cratersRadius.Add(Random.Range(settings.craterSettings.craterRadiusMin, settings.craterSettings.craterRadiusMax));
                cratersFloor.Add(Random.Range(settings.craterSettings.floorHeightMin, settings.craterSettings.floorHeightMax));
            }
        }
        else     //If number of craters doesn't change, redraw the craters with the new settings
		{
            cratersRadius.Clear();
            cratersFloor.Clear();
            for (int i = 0; i < settings.craterSettings.numCraters; i++)    //Enter list entities for each of the crater settings
            {
                cratersRadius.Add(Random.Range(settings.craterSettings.craterRadiusMin, settings.craterSettings.craterRadiusMax));
                cratersFloor.Add(Random.Range(settings.craterSettings.floorHeightMin, settings.craterSettings.floorHeightMax));
            }
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)    //Calculate height at specific point
    {
        layerValues.Add(0);
        float elevation = 0;
        float craterElevation = 0;
        
        for (int i = 0; i < settings.craterSettings.numCraters; i++)    //Loop over each crater
        {
            float x = (pointOnUnitSphere - craters[i]).magnitude / cratersRadius[i];    //Specific point

            float hole = Mathf.Pow(x, 2) - 1; //Crater hole
            float r = Mathf.Min(x - settings.craterSettings.rimWidth - 1, 0);    //No values larger than 0 for the rim
            float rim = settings.craterSettings.rimSteepness * Mathf.Pow(r, 2);  //Final rim height

            float craterShape = Mathf.Max(hole, cratersFloor[i]);   //Final crater shape between hole and floor
            craterShape = Mathf.Min(craterShape, rim);  //Final crater shape between the previous shape and the rim
            craterElevation = Mathf.Min(craterShape, craterElevation);             //Final crater elevation is the most deep crater (Multiple craters can occur in the same spot essentially)
        }

        if (noiseFilters.Length > 0)    //If we have a previous layer
        {
            layerValues[0] = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)    //User layer as a mask            
                elevation = layerValues[0];            
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].usePrevLayerAsMask) ? layerValues[0] : 1; //If we should be using a mask at this point

                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;    //Multiple by our mask layer
            }
            layerValues.Add(noiseFilters[i].Evaluate(pointOnUnitSphere));    //Update current layer mask
        }

        elevation = settings.planetRadius * (1 + elevation) + craterElevation;  //Elevation is a combination of terrain + crater modification
        elevationMinMax.AddValue(elevation);    //Update the shader height value

        return pointOnUnitSphere * elevation;   //Return the vertex point to change
    }
}

