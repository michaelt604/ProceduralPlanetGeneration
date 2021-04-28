using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public enum PlanetType { Gas, Earth, Rocky, Cracked }
    public PlanetType planetType;   //Current selection
    PlanetType lastType = PlanetType.Gas;   //Check if selection changed

    public GameObject gasGiantPrefab;
    public GameObject earthLikePrefab;
    public GameObject rockyPrefab;
    public GameObject crackedPrefab;

    public GameObject curPlanet;

    // Start is called before the first frame update
    void Start()
    {
        //curPlanet = Instantiate(gasGiantPrefab, new Vector3(0, 0, 10), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (planetType != lastType)
		{
            lastType = planetType;
            Destroy(curPlanet);

            resetPositions();

            if (planetType == PlanetType.Gas)
			{
                gasGiantPrefab.transform.position = new Vector3(0, 0, 10);
                
            }
            else if (planetType == PlanetType.Earth)
            {
                earthLikePrefab.transform.position = new Vector3(0, 0, 10);
            }
            else if (planetType == PlanetType.Rocky)
            {
                rockyPrefab.transform.position = new Vector3(0, 0, 10);
            }
            else if (planetType == PlanetType.Cracked)
            {
                crackedPrefab.transform.position = new Vector3(0, 0, 10);
            }
        }
    }

    void resetPositions()   //Reset all planet positions
	{
        gasGiantPrefab.transform.position = new Vector3(0, 0, 100);
        earthLikePrefab.transform.position = new Vector3(0, 0, 100);
        rockyPrefab.transform.position = new Vector3(0, 0, 100);
        crackedPrefab.transform.position = new Vector3(0, 0, 100);

    }
}

