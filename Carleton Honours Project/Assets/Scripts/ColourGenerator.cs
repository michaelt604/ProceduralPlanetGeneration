using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator 
{
    ColourSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter latNoiseFilter;

    public void UpdateSettings(ColourSettings settings)
    {
        this.settings = settings;
        if (texture == null || texture.height != settings.latitudeSettings.latitudes.Length)    //Reevaluate if we don't have a texture or our latitude settings change
        {
            texture = new Texture2D(textureResolution, settings.latitudeSettings.latitudes.Length, TextureFormat.RGBA32, false);    //Create texture based on height and number of latitude regions
        }
        latNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.latitudeSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float LatPercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;   //0 on south pole, 1 on north pole, need to change later
        heightPercent += (latNoiseFilter.Evaluate(pointOnUnitSphere) - settings.latitudeSettings.noiseOffset) * settings.latitudeSettings.noiseStrength;    //Modify latitude based on noise settings

        float biomeIndex = 0;


        //int numBiomes = settings.latitudeSettings.latitudes.Length;
        int numLats = settings.latitudeSettings.latitudes.Length;
        //int numLats = settings.latitudeSettings.NewLats(settings.latitudeSettings.latitudes).Length;    //Number of duplicated latitudes
        float blendRange = settings.latitudeSettings.blendAmount / 2f + .001f;

        for (int j = 0; j < numLats; j++) //For each latitude
        {
            float latChange = settings.latitudeSettings.NewLats(settings.latitudeSettings.latitudes)[j].latitudePercent;
            float dst = heightPercent - latChange;

            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += j * weight;            
        }        

        return biomeIndex / Mathf.Max(1, (numLats - 1));
    }

    public void UpdateColours()
    {
        Color[] colours = new Color[textureResolution * texture.height];
        int colourIndex = 0;

        //ColourSettings oldSettings = settings;
        //settings.latitudeSettings.latitudes = oldSettings.latitudeSettings.NewLats(oldSettings.latitudeSettings.latitudes); //Doubles latitudes size

        foreach(var lat in settings.latitudeSettings.latitudes)
        {            
            for (int i = 0; i < textureResolution; i++)
            {
                Color gradientCol = lat.latitudeColour.Evaluate(i / (textureResolution - 1f));
                Color tintCol = lat.test;
                colours[colourIndex] = gradientCol * (1 - lat.tintPercent) + tintCol * lat.tintPercent;
                colourIndex++;
            }
        }
        //settings = oldSettings; //Resets settings

        texture.SetPixels(colours);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);

    }
}
