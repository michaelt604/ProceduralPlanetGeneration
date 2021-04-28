using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasGiant : MonoBehaviour
{
    public Texture2D gasTexture;
    public Gradient gasGrad;
    public GradientColorKey[] colorKey;
    public GradientAlphaKey[] alphaKey;

    public float biasLight = 0.1f;
    public float biasDark = 0.2f;
    public int biasRangeMin = 400;
    public int biasRangeMax = 1000;
    [Range(0, 10)]
    public int blurAmountRadius = 2;
    [Range(0, 10)]
    public int blurAmountIteration = 2;
    public int seed = 1000;


    float oldBiasLight = 0;
    float oldBiasDark = 0;
    int oldBiasRangeMin = 0;
    int oldBiasRangeMax = 0;
    int oldBlurAmountRad = 0;
    int oldBlurAmountIter = 0;
    int oldSeed = 0;

    public Material material;
    private Material thisMaterial;

	private void Awake()
	{
        thisMaterial = (Material)Instantiate(this.material);   //Instance material for just this planet
        Random.InitState(seed);   //Random seed
        oldSeed = seed;
    }

	private void Update()   //Update texture when setting is updated
	{
        bool changed = false;

		if (oldSeed != seed)    //Seed changed, redo start
		{
            oldSeed = seed;
            Random.InitState(seed);
            changed = true;
		}
        if (oldBiasLight != biasLight)
		{
            oldBiasLight = biasLight;
            Random.InitState(seed);
            changed = true;
		}
        if (oldBiasDark != biasDark)
        {
            oldBiasDark = biasDark;
            Random.InitState(seed);
            changed = true;
        }
        if (oldBiasRangeMin != biasRangeMin)
        {
            oldBiasRangeMin = biasRangeMin;
            Random.InitState(seed);
            changed = true;
        }
        if (oldBiasRangeMax != biasRangeMax)
        {
            oldBiasRangeMax = biasRangeMax;
            Random.InitState(seed);
            changed = true;
        }
        if (oldBlurAmountIter != blurAmountIteration)
        {
            oldBlurAmountIter = blurAmountIteration;
            Random.InitState(seed);
            changed = true;
        }
        if (oldBlurAmountRad != blurAmountRadius)
        {
            oldBlurAmountRad = blurAmountRadius;
            Random.InitState(seed);
            changed = true;
        }

        if (changed)
            Start();

    }

	// Start is called before the first frame update
	void Start()
    {

        int height = 2500;                      //Number of pixels in the texture
        gasTexture = new Texture2D(1, height);  //Create our new texture with a height of 2500, and width of 1
        gasTexture.name = "Gas Giant";          //Give our texture its name

        int biasDirection = Random.Range(-1, 1);    //Get our bias direction, ensuring its non zero
        while (biasDirection == 0)
            biasDirection = Random.Range(-1, 1);

        int minOff = (int)Mathf.Round(height / 5.0f);   //Minimum offset
        int maxOff = (int)Mathf.Round(height / 2.5f);   //Maximum offset

        int randomDarkOffsetTop = Random.Range(minOff, maxOff);
        int randomDarkOffsetBot = height - Random.Range(minOff, maxOff);


        for (int i = 0; i < height; i++)    //Loop through all height pixels
        {
            int biasLength = Random.Range(biasRangeMin, biasRangeMax); //Bias between light and dark
            biasDirection *= -1;    //Flip bias direction for each large bar

            if (biasDirection > 0)                
                biasLength = (int)Mathf.Round(biasLength * Random.Range(1.1f, 1.3f));  //Light areas longer than dark    
                
            for (int j = i; j < i + biasLength; j++)    //Loop through the bias length pixels
            {
                int barLength = Random.Range(5, 10);    //Range number of pixels
                float randColour = Random.Range(0.0f, 1.0f);  //Colour chosen for the range

                float biasAmount = biasDirection * Random.Range(biasLight, biasDark);
                for (int k = j; k < j + barLength; k++)
                {
                    Color curColour = gasGrad.Evaluate(randColour);
                    curColour += new Color(biasAmount, biasAmount, biasAmount);


                    if (k < height) //Ensure we don't write out of bounds
                    {
                        if (k < randomDarkOffsetTop)
                            curColour *= 0.7f;   //Dark bias towards top
                        else if (randomDarkOffsetBot < k)
                            curColour *= 0.7f;  //Dark bias towards bottom
                            
                        gasTexture.SetPixel(0, k, curColour);  //Set pixel colour       
                    }
                }
                j += barLength; //Increment j towards textureheight
            }
            i += biasLength;    //INcrement i towards textureheight
        }

        LinearBlur linBur = new LinearBlur();
        gasTexture = linBur.Blur(gasTexture, blurAmountRadius, blurAmountIteration);
        gasTexture.Apply();    //Sets changes


        MeshRenderer rend = GetComponent<MeshRenderer>();   //Get current mesh renderer
        rend.material = thisMaterial;
        rend.material.SetFloat("_Seed", seed);    //Sets seed
        rend.material.SetFloat("_StormSize", 4f); //Sets storm
        rend.material.SetTexture("_PlanetTexture", gasTexture); //Sets texture in material
    }

  
}


/* FROM https://forum.unity.com/threads/contribution-texture2d-blur-in-c.185694/ 
Specifically Cardinalby's blur solution*/
class LinearBlur
{
    private float _rSum = 0;
    private float _gSum = 0;
    private float _bSum = 0;

    private Texture2D _sourceImage;
    private int _sourceWidth;
    private int _sourceHeight;
    private int _windowSize;

    public Texture2D Blur(Texture2D image, int radius, int iterations)
    {
        _windowSize = radius * 2 + 1;
        _sourceWidth = image.width;
        _sourceHeight = image.height;

        var tex = image;

        for (var i = 0; i < iterations; i++)
        {
            tex = OneDimensialBlur(tex, radius, true);
            tex = OneDimensialBlur(tex, radius, false);
        }

        return tex;
    }

    private Texture2D OneDimensialBlur(Texture2D image, int radius, bool horizontal)
    {
        _sourceImage = image;

        var blurred = new Texture2D(image.width, image.height, image.format, false);

        if (horizontal)
        {
            for (int imgY = 0; imgY < _sourceHeight; ++imgY)
            {
                ResetSum();

                for (int imgX = 0; imgX < _sourceWidth; imgX++)
                {
                    if (imgX == 0)
                        for (int x = radius * -1; x <= radius; ++x)
                            AddPixel(GetPixelWithXCheck(x, imgY));
                    else
                    {
                        var toExclude = GetPixelWithXCheck(imgX - radius - 1, imgY);
                        var toInclude = GetPixelWithXCheck(imgX + radius, imgY);

                        SubstPixel(toExclude);
                        AddPixel(toInclude);
                    }

                    blurred.SetPixel(imgX, imgY, CalcPixelFromSum());
                }
            }
        }

        else
        {
            for (int imgX = 0; imgX < _sourceWidth; imgX++)
            {
                ResetSum();

                for (int imgY = 0; imgY < _sourceHeight; ++imgY)
                {
                    if (imgY == 0)
                        for (int y = radius * -1; y <= radius; ++y)
                            AddPixel(GetPixelWithYCheck(imgX, y));
                    else
                    {
                        var toExclude = GetPixelWithYCheck(imgX, imgY - radius - 1);
                        var toInclude = GetPixelWithYCheck(imgX, imgY + radius);

                        SubstPixel(toExclude);
                        AddPixel(toInclude);
                    }

                    blurred.SetPixel(imgX, imgY, CalcPixelFromSum());
                }
            }
        }

        blurred.Apply();
        return blurred;
    }

    private Color GetPixelWithXCheck(int x, int y)
    {
        if (x <= 0) return _sourceImage.GetPixel(0, y);
        if (x >= _sourceWidth) return _sourceImage.GetPixel(_sourceWidth - 1, y);
        return _sourceImage.GetPixel(x, y);
    }

    private Color GetPixelWithYCheck(int x, int y)
    {
        if (y <= 0) return _sourceImage.GetPixel(x, 0);
        if (y >= _sourceHeight) return _sourceImage.GetPixel(x, _sourceHeight - 1);
        return _sourceImage.GetPixel(x, y);
    }

    private void AddPixel(Color pixel)
    {
        _rSum += pixel.r;
        _gSum += pixel.g;
        _bSum += pixel.b;
    }

    private void SubstPixel(Color pixel)
    {
        _rSum -= pixel.r;
        _gSum -= pixel.g;
        _bSum -= pixel.b;
    }

    private void ResetSum()
    {
        _rSum = 0.0f;
        _gSum = 0.0f;
        _bSum = 0.0f;
    }

    Color CalcPixelFromSum()
    {
        return new Color(_rSum / _windowSize, _gSum / _windowSize, _bSum / _windowSize);
    }
}