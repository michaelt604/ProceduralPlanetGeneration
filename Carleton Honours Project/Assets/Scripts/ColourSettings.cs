using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject 
{
    public Material planetMaterial;
    public LatitudeSettings latitudeSettings;
    //int height = 2500 / 2;  //Max height / 2 so our height is only for one hemisphere (the other hemisphere is mirrored).

    [System.Serializable]
    public class LatitudeSettings
	{
        public Latitude[] latitudes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendAmount;

        [System.Serializable]
        public class Latitude
        {
            public Gradient latitudeColour;
            public Color test;
            [Range(0, 1)]
            public float latitudePercent;
            [Range(0, 1)]
            public float tintPercent;
        }

        public Latitude[] NewLats(Latitude[] oldLats)
		{
            int a = oldLats.Length * 2;
            Latitude[] newLatArr = new Latitude[a];

            for (int i = 0; i < oldLats.Length; i++)
			{
                newLatArr[i] = oldLats[i];
                newLatArr[oldLats.Length - i] = oldLats[i];
			}

            return newLatArr;
		}
	}
} 
